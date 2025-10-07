using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.Services.WebApi.Enums;
using TodoListApp.WebApp.CustomLogs;
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
            var userId = this.GetCurrentUserId();
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
            var filter = ParseTaskFilter(taskFilter);

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
            viewModel.Tasks = tasks.Select(t => MapToTaskViewModel(t)).ToList();
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
            return this.View("Error");
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, int? listId, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = this.GetCurrentUserId();
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

            var viewModel = new TodoTaskDetailsViewModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreationDate = task.CreationDate ?? DateTime.MinValue,
                DueDate = task.DueDate,
                Status = task.Status?.StatusTitle ?? "Unknown",
                StatusId = task.StatusId,
                OwnerName = FormatOwnerName(task.OwnerUser?.FirstName, task.OwnerUser?.LastName),
                Tags = task.UsersTags?.Select(t => new TagViewModel() { Id = t.Id, Title = t.Title }).ToList() ?? new List<TagViewModel>(),
                Comments = task.UserComments?.Select(c => c.Text).ToList() ?? new List<string>(),
                ListId = listId,
                Role = StringToRoleEnum(list.UserRole ?? "None"),
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
            return this.View("Error");
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? title, DateTime? creationDate, DateTime? dueDate, int? pageNumber, int? rowCount)
    {
        try
        {
            var userId = this.GetCurrentUserId();
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
            viewModel.Tasks = tasks.Select(t => MapToTaskViewModel(t)).ToList();

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
            return this.View("Error");
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create(int listId, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = this.GetCurrentUserId();
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
            return this.View("Error");
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTodoTaskViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        try
        {
            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var taskModel = new TodoTaskModel(
                id: 0,
                title: model.Title,
                description: model.Description ?? string.Empty,
                creationDate: null,
                dueDate: model.DueDate,
                statusId: (int)model.Status,
                ownerUserId: userId.Value,
                listId: model.ListId);

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

    [HttpGet]
    public async Task<IActionResult> Edit(int id, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = this.GetCurrentUserId();
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
            return this.View("Error");
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditTodoTaskViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        try
        {
            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var taskModel = new TodoTaskModel(
                id: model.TaskId,
                title: model.Title,
                description: model.Description ?? string.Empty,
                creationDate: null,
                dueDate: model.DueDate,
                statusId: (int)model.Status,
                ownerUserId: userId.Value,
                listId: model.ListId);

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, TodoTaskStatus status, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = this.GetCurrentUserId();
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
            var userId = this.GetCurrentUserId();
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

    private static TaskFilter ParseTaskFilter(string? filter)
    {
        return filter?.ToUpperInvariant() switch
        {
            "NOTSTARTED" => TaskFilter.NotStarted,
            "INPROGRESS" => TaskFilter.InProgress,
            "COMPLETED" => TaskFilter.Completed,
            "ALL" => TaskFilter.All,
            _ => TaskFilter.Active
        };
    }

    private static TodoTaskViewModel MapToTaskViewModel(TodoTaskModel model)
    {
        return new TodoTaskViewModel
        {
            TaskId = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreationDate = model.CreationDate ?? DateTime.MinValue,
            DueDate = model.DueDate,
            Status = model.Status?.StatusTitle ?? "Unknown",
            ListId = model.ListId,
            OwnerName = FormatOwnerName(model.OwnerUser?.FirstName, model.OwnerUser?.LastName),
            Tags = model.UsersTags?.Select(tag => tag.Title) ?? Enumerable.Empty<string>(),
        };
    }

    private static string FormatOwnerName(string? firstName, string? lastName)
    {
        if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
        {
            return "N/A";
        }

        var name = firstName ?? "N/A";
        if (!string.IsNullOrEmpty(lastName))
        {
            name += $" {lastName[0]}.";
        }

        return name;
    }

    private static ListRole StringToRoleEnum(string roleName)
    {
        return roleName.ToUpperInvariant() switch
        {
            "OWNER" => ListRole.Owner,
            "EDITOR" => ListRole.Editor,
            "VIEWER" => ListRole.Viewer,
            _ => ListRole.None,
        };
    }

    private int? GetCurrentUserId()
    {
        var userNameIdentifier = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userNameIdentifier != null &&
            int.TryParse(userNameIdentifier, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
        {
            return userId;
        }

        return null;
    }
}
