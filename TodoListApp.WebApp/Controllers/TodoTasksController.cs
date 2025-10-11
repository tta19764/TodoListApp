using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.Services.WebApi.Enums;
using TodoListApp.WebApp.CustomLogs;
using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models.Comment;
using TodoListApp.WebApp.Models.Tag;
using TodoListApp.WebApp.Models.Task;

namespace TodoListApp.WebApp.Controllers;
[Authorize]
public class TodoTasksController : Controller
{
    private readonly ITodoTaskService todoTaskService;
    private readonly ITodoListService todoListService;
    private readonly IAssignedTasksService assignedTasksService;
    private readonly ILogger<TodoTasksController> logger;
    private readonly ISearchTasksService searchTasksService;

    public TodoTasksController(
        ITodoTaskService todoTaskService,
        ITodoListService todoListService,
        ILogger<TodoTasksController> logger,
        IAssignedTasksService assignedTasksService,
        ISearchTasksService searchTasksService)
    {
        this.todoTaskService = todoTaskService ?? throw new ArgumentNullException(nameof(todoTaskService));
        this.todoListService = todoListService ?? throw new ArgumentNullException(nameof(todoListService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.assignedTasksService = assignedTasksService ?? throw new ArgumentNullException(nameof(assignedTasksService));
        this.searchTasksService = searchTasksService ?? throw new ArgumentNullException(nameof(searchTasksService));
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? taskFilter, string? sortBy, string? sortOrder, int? pageNumber, int? rowCount)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var viewModel = new AssignedTasksViewModel
            {
                TaskFilter = taskFilter ?? "active",
                SortBy = sortBy ?? "Id",
                SortOrder = sortOrder ?? "asc",
                PageNumber = pageNumber ?? 1,
                RowCount = rowCount ?? 10,
            };

            // Parse task filter
            var filter = Formaters.StringToTaskFilterEnum(taskFilter);

            var totalCount = await this.assignedTasksService.GetAllAssignedCountAsync(userId.Value, filter);
            viewModel.TotalPages = (int)Math.Ceiling(totalCount / (double)viewModel.RowCount);

            // Get all tasks assigned to the user
            var tasks = await this.assignedTasksService.GetAllAssignedAsync(
                userId.Value,
                filter,
                viewModel.SortBy,
                viewModel.SortOrder,
                viewModel.PageNumber,
                viewModel.RowCount);

            // Map to view models
            viewModel.Tasks = tasks.Select(t => MapToViewModel.ToTask(userId.Value, t)).ToList();
            viewModel.TotalTasks = viewModel.Tasks.Count();

            if (this.ModelState.IsValid)
            {
                TodoTasksLog.LogAssignedTasksRetrieved(this.logger, viewModel.TotalTasks, userId.Value);
                return this.View(viewModel);
            }

            return this.View("Error");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorLoadingAssignedTasks(this.logger, ex);
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, int? listId, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var task = await this.todoTaskService.GetByIdAsync(userId.Value, id);
            if (task == null)
            {
                TodoTasksLog.LogTaskNotFoundForUser(this.logger, id, userId.Value);
                return this.NotFound();
            }

            var list = await this.todoListService.GetByIdAsync(userId.Value, task.ListId);
            if (list == null)
            {
                TodoListsLog.LogListNotFoundForUser(this.logger, id, userId.Value);
                return this.NotFound();
            }

            TodoTasksLog.LogTaskDetailsRetrieved(this.logger, id, userId.Value);

            if (returnUrl == null)
            {
                if (listId.HasValue)
                {
                    returnUrl = new Uri(this.Url.Action("Index", "TodoLists", new { listId }) ?? "~/", UriKind.RelativeOrAbsolute);
                }
                else
                {
                    returnUrl = new Uri(this.Url.Action("Index", "TodoLists") ?? "~/", UriKind.RelativeOrAbsolute);
                }
            }

            var comments = await this.todoTaskService.GetTaskCommentsAsync(id, userId.Value);

            var viewModel = new TodoTaskDetailsViewModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreationDate = task.CreationDate ?? DateTime.MinValue,
                DueDate = task.DueDate,
                Status = task.Status?.StatusTitle ?? "Unknown",
                StatusId = task.StatusId,
                OwnerName = Formaters.FormatOwnerName(task.OwnerUser?.FirstName, task.OwnerUser?.LastName),
                Tags = task.UsersTags?.Select(t => new TagViewModel() { Id = t.Id, Title = t.Title }).ToList() ?? new List<TagViewModel>(),
                Comments = comments.Select(c =>
                {
                    var comment = MapToViewModel.ToComment(userId.Value, c);
                    if (Formaters.StringToRoleEnum(list.UserRole) == ListRole.Owner)
                    {
                        comment.CanEdit = true;
                    }

                    return comment;
                }).ToList(),
                ListId = listId,
                Role = Formaters.StringToRoleEnum(list.UserRole),
                ReturnUrl = returnUrl,
            };

            if (this.ModelState.IsValid)
            {
                TodoTasksLog.LogTaskDetailsRetrieved(this.logger, id, userId.Value);
                return this.View(viewModel);
            }

            return this.View("Error");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorRetrievingTaskDetails(this.logger, id, ex);
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> AddComment(int taskId, Uri? returnUrl)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var task = await this.todoTaskService.GetByIdAsync(userId.Value, taskId);
            if (task == null)
            {
                TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, userId.Value);
                return this.NotFound();
            }

            var list = await this.todoListService.GetByIdAsync(userId.Value, task.ListId);
            if (list == null)
            {
                return this.NotFound();
            }

            var role = Formaters.StringToRoleEnum(list.UserRole ?? "None");
            if (role != ListRole.Owner && role != ListRole.Editor)
            {
                return this.Forbid();
            }

            var viewModel = new AddCommentViewModel
            {
                TaskId = taskId,
                TaskTitle = task.Title,
                ReturnUrl = returnUrl ?? new Uri(this.Url.Action("Details", new { id = taskId }) ?? "~/", UriKind.RelativeOrAbsolute),
            };

            if (this.ModelState.IsValid)
            {
                TodoTasksLog.LogAddCommentPageLoaded(this.logger, taskId);
                return this.View(viewModel);
            }

            return this.View("Error");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorLoadingAddCommentPage(this.logger, taskId, ex);
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> AddComment(AddCommentViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (!this.ModelState.IsValid)
        {
            return this.AddCommentInternal(model);
        }

        return Task.FromResult<IActionResult>(this.View(model));
    }

    [HttpGet]
    public async Task<IActionResult> EditComment(int commentId, int taskId, Uri? returnUrl)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var task = await this.todoTaskService.GetByIdAsync(userId.Value, taskId);
            if (task == null)
            {
                TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, userId.Value);
                return this.NotFound();
            }

            var list = await this.todoListService.GetByIdAsync(userId.Value, task.ListId);
            if (list == null)
            {
                return this.NotFound();
            }

            var comment = await this.todoTaskService.GetCommentByIdAsync(userId.Value, taskId, commentId);
            if (comment == null)
            {
                TodoTasksLog.LogCommentNotFoundForUser(this.logger, commentId, userId.Value);
                return this.NotFound();
            }

            var role = Formaters.StringToRoleEnum(list.UserRole);
            if (role != ListRole.Owner && userId.Value != comment.UserId)
            {
                return this.Forbid();
            }

            var viewModel = new EditCommentViewModel
            {
                CommentId = commentId,
                TaskId = taskId,
                TaskTitle = task.Title,
                Text = comment.Text,
                ReturnUrl = returnUrl ?? new Uri(this.Url.Action("Details", new { id = taskId }) ?? "~/", UriKind.RelativeOrAbsolute),
            };

            if (this.ModelState.IsValid)
            {
                TodoTasksLog.LogEditCommentPageLoaded(this.logger, commentId);
                return this.View(viewModel);
            }

            return this.View("Error");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorLoadingEditCommentPage(this.logger, commentId, ex);
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> EditComment(EditCommentViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (this.ModelState.IsValid)
        {
            return this.EditCommentInternal(model);
        }

        return Task.FromResult<IActionResult>(this.View(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteComment(int commentId, int taskId, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var task = await this.todoTaskService.GetByIdAsync(userId.Value, taskId);
            if (task == null)
            {
                TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, userId.Value);
                return this.NotFound();
            }

            var list = await this.todoListService.GetByIdAsync(userId.Value, task.ListId);
            if (list == null)
            {
                return this.NotFound();
            }

            var comment = await this.todoTaskService.GetCommentByIdAsync(userId.Value, taskId, commentId);
            if (comment == null)
            {
                TodoTasksLog.LogCommentNotFoundForUser(this.logger, commentId, userId.Value);
                return this.NotFound();
            }

            var role = Formaters.StringToRoleEnum(list.UserRole);
            if (role != ListRole.Owner && userId.Value != comment.UserId)
            {
                return this.Forbid();
            }

            await this.todoTaskService.RemoveTaskCommentAsync(userId.Value, taskId, commentId);

            TodoTasksLog.LogCommentDeleted(this.logger, commentId, userId.Value);

            this.TempData["SuccessMessage"] = "Comment deleted successfully";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.RedirectToAction("Details", new { id = taskId });
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorDeletingComment(this.logger, commentId, ex);
            this.TempData["ErrorMessage"] = "An error occurred while deleting the comment.";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.RedirectToAction("Details", new { id = taskId });
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? title, DateTime? creationDate, DateTime? dueDate, int? pageNumber, int? rowCount)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var viewModel = new SearchTasksViewModel
            {
                Title = title,
                CreationDate = creationDate,
                DueDate = dueDate,
                PageNumber = pageNumber ?? 1,
                RowCount = rowCount ?? 10,
            };

            // Get total count
            var totalCount = await this.searchTasksService.GetAllSearchCountAsync(
                userId.Value,
                title,
                creationDate,
                dueDate);

            viewModel.TotalPages = (int)Math.Ceiling(totalCount / (double)viewModel.RowCount);
            viewModel.TotalTasks = totalCount;

            // Get search results
            var tasks = await this.searchTasksService.SearchTasksAsync(
                userId.Value,
                title,
                creationDate,
                dueDate,
                viewModel.PageNumber,
                viewModel.RowCount);

            // Map to view models
            viewModel.Tasks = tasks.Select(t => MapToViewModel.ToTask(userId.Value, t)).ToList();

            if (this.ModelState.IsValid)
            {
                TodoTasksLog.LogSearchTasksRetrieved(this.logger, viewModel.TotalTasks, userId.Value);
                return this.View(viewModel);
            }

            return this.View("Error");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorLoadingSearchedTasks(this.logger, ex);
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create(int listId, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            // Verify user has access to the list
            var list = await this.todoListService.GetByIdAsync(userId.Value, listId);
            if (list == null)
            {
                TodoTasksLog.LogListNotFoundForUser(this.logger, listId, userId.Value);
                return this.NotFound();
            }

            TodoTasksLog.LogCreatePageLoadedSuccessfully(this.logger, listId);

            if (returnUrl == null)
            {
                returnUrl = new Uri(this.Url.Action("Index", "TodoLists", new { listId }) ?? "~/", UriKind.RelativeOrAbsolute);
            }

            var viewModel = new CreateTodoTaskViewModel
            {
                ListId = listId,
                ListTitle = list.Title,
                Status = TodoTaskStatus.NotStarted, // Default to "Not Started"
                DueDate = DateTime.Today.AddDays(7),
                ReturnUrl = returnUrl,
            };

            return this.View(viewModel);
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorLoadingCreatePage(this.logger, listId, ex);
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Create(CreateTodoTaskViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (this.ModelState.IsValid)
        {
            return this.CreateInternal(model);
        }

        return Task.FromResult<IActionResult>(this.View(model));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var task = await this.todoTaskService.GetByIdAsync(userId.Value, id);
            if (task == null)
            {
                TodoTasksLog.LogTaskNotFoundForEdit(this.logger, id, userId.Value);
                return this.NotFound();
            }

            TodoTasksLog.LogEditPageLoadedSuccessfully(this.logger, id);

            var viewModel = new EditTodoTaskViewModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = (TodoTaskStatus)task.StatusId,
                ListId = task.ListId,
                ReturnUrl = returnUrl ?? new Uri(this.Url.Action("Details", new { id }) ?? "~/"),
            };

            return this.View(viewModel);
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorLoadingEditPage(this.logger, id, ex);
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(EditTodoTaskViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (this.ModelState.IsValid)
        {
            return this.EditInternal(model);
        }

        return Task.FromResult<IActionResult>(this.View(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, TodoTaskStatus status, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var updatedTask = await this.todoTaskService.UpdateTaskStatusAsync(userId.Value, id, (int)status);

            TodoTasksLog.LogTaskStatusUpdated(this.logger, id, (int)status, userId.Value);

            // Use the returned task model if needed (e.g., to display success message with task details)
            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.RedirectToAction("Index", "TodoLists", new { updatedTask.ListId });
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorUpdatingTaskStatus(this.logger, id, ex);
            return this.RedirectToAction("Details", new { id });
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int? listId, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            await this.todoTaskService.DeleteAsync(userId.Value, id);

            TodoTasksLog.LogTaskDeletedSuccessfully(this.logger, id, userId.Value);

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            if (listId.HasValue)
            {
                return this.RedirectToAction("Index", "TodoLists", new { listId });
            }

            return this.RedirectToAction("Index", "TodoLists");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorDeletingTask(this.logger, id, ex);

            if (listId.HasValue)
            {
                return this.RedirectToAction("Index", "TodoLists", new { listId });
            }

            return this.RedirectToAction("Index", "TodoLists");
            throw;
        }
    }

    private async Task<IActionResult> AddCommentInternal(AddCommentViewModel model)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var commentModel = new CommentModel(0, model.Text, model.TaskId, userId.Value);
            var createdComment = await this.todoTaskService.AddTaskCommentAsync(commentModel);

            TodoTasksLog.LogCommentAddedToTask(this.logger, createdComment.Id, model.TaskId, userId.Value);

            this.TempData["SuccessMessage"] = "Comment added successfully";
            return this.Redirect(model.ReturnUrl.ToString());
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorAddingComment(this.logger, model.TaskId, ex);
            this.ModelState.AddModelError(string.Empty, "An error occurred while adding the comment.");
            throw;
        }
    }

    private async Task<IActionResult> EditCommentInternal(EditCommentViewModel model)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var commentModel = new CommentModel(model.CommentId, model.Text, model.TaskId, userId.Value);
            var updatedComment = await this.todoTaskService.UpdateTaskCommentAsync(userId.Value, commentModel);

            TodoTasksLog.LogCommentUpdated(this.logger, updatedComment.Id, userId.Value);

            this.TempData["SuccessMessage"] = "Comment updated successfully";
            return this.Redirect(model.ReturnUrl.ToString());
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorUpdatingComment(this.logger, model.CommentId, ex);
            this.ModelState.AddModelError(string.Empty, "An error occurred while updating the comment.");
            throw;
        }
    }

    private async Task<IActionResult> CreateInternal(CreateTodoTaskViewModel model)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var taskModel = new TodoTaskModel(0)
            {
                Title = model.Title,
                Description = model.Description ?? string.Empty,
                CreationDate = null,
                DueDate = model.DueDate,
                StatusId = (int)model.Status,
                OwnerUserId = userId.Value,
                ListId = model.ListId,
            };

            _ = await this.todoTaskService.AddAsync(taskModel);

            TodoTasksLog.LogTaskCreatedSuccessfully(this.logger, model.ListId, userId.Value);

            return this.Redirect(model.ReturnUrl.ToString());
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorCreatingTask(this.logger, model.ListId, ex);
            this.ModelState.AddModelError(string.Empty, "An error occurred while creating the task.");
            return this.View(model);
            throw;
        }
    }

    private async Task<IActionResult> EditInternal(EditTodoTaskViewModel model)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var taskModel = new TodoTaskModel(model.TaskId)
            {
                Title = model.Title,
                Description = model.Description ?? string.Empty,
                CreationDate = null,
                DueDate = model.DueDate,
                StatusId = (int)model.Status,
                OwnerUserId = userId.Value,
                ListId = model.ListId,
            };

            _ = await this.todoTaskService.UpdateAsync(userId.Value, taskModel);

            TodoTasksLog.LogTaskUpdatedSuccessfully(this.logger, model.TaskId, userId.Value);

            // Use the returned task model if needed to display updated data
            return this.Redirect(model.ReturnUrl.ToString());
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogErrorUpdatingTask(this.logger, model.TaskId, ex);
            this.ModelState.AddModelError(string.Empty, "An error occurred while updating the task.");
            return this.View(model);
            throw;
        }
    }
}
