using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.WebApp.CustomLogs;
using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models.List;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class TodoListsController : Controller
{
    private readonly ITodoListService todoListService;
    private readonly ITodoTaskService todoTaskService;
    private readonly ILogger<TodoListsController> logger;

    public TodoListsController(
        ITodoListService todoListService,
        ITodoTaskService todoTaskService,
        ILogger<TodoListsController> logger)
    {
        this.todoListService = todoListService ?? throw new ArgumentNullException(nameof(todoListService));
        this.todoTaskService = todoTaskService ?? throw new ArgumentNullException(nameof(todoTaskService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? listId, string? listFilter, string? taskFilter, int? listPageNumber, int? listRowCount, int? taskPageNumber, int? taskRowCount)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            // count depending on filter
            int totalCount;
            if (listFilter == "owned")
            {
                totalCount = await this.todoListService.AllByAuthorCount(userId.Value);
            }
            else if (listFilter == "shared")
            {
                totalCount = await this.todoListService.AllSharedCount(userId.Value);
            }
            else
            {
                totalCount = await this.todoListService.AllByUserCount(userId.Value);
            }

            var viewModel = new TodoListsPageViewModel
            {
                SelectedListId = listId,
                ListFilter = listFilter ?? "all",
                TaskFilter = taskFilter ?? "active",
                ListPageNumber = listPageNumber ?? 1,
                ListRowCount = listRowCount ?? 5,
            };

            viewModel.ListTotalPages = (int)Math.Ceiling(totalCount / (double)viewModel.ListRowCount);

            // load lists with paging
            IReadOnlyList<TodoListModel> lists;
            if (listFilter == "owned")
            {
                lists = await this.todoListService.GetAllByAuthorAsync(userId.Value, viewModel.ListPageNumber, viewModel.ListRowCount);
            }
            else if (listFilter == "shared")
            {
                // you may need a GetAllSharedAsync(userId, page, rowCount)
                lists = await this.todoListService.GetAllSharedAsync(userId.Value, viewModel.ListPageNumber, viewModel.ListRowCount);
            }
            else
            {
                lists = await this.todoListService.GetAllAsync(userId.Value, viewModel.ListPageNumber, viewModel.ListRowCount);
            }

            TodoListsLog.LogListsRetrievedForUser(this.logger, lists.Count, userId.Value);

            // Map to view models
            viewModel.TodoLists = lists.Select(l => MapToViewModel.ToList(l)).AsEnumerable();

            // If a list is selected, load its tasks
            if (listId.HasValue)
            {
                TodoListModel? selectedList = null;

                // Get the selected list details
                if (listId.Value != viewModel.SelectedListId)
                {
                    selectedList = lists.FirstOrDefault(l => l.Id == listId.Value);
                }
                else
                {
                    selectedList = await this.todoListService.GetByIdAsync(userId.Value, listId.Value);
                }

                if (selectedList != null)
                {
                    viewModel.SelectedList = MapToViewModel.ToList(selectedList);

                    // Parse task filter
                    var filter = Formaters.StringToTaskFilterEnum(taskFilter);

                    viewModel.TaskPageNumber = taskPageNumber ?? 1;
                    viewModel.TaskRowCount = taskRowCount ?? 10;

                    totalCount = await this.todoTaskService.GetAllByListIdCountAsync(listId.Value, userId.Value, filter);
                    viewModel.TaskTotalPages = (int)Math.Ceiling(totalCount / (double)viewModel.TaskRowCount);

                    // Load tasks
                    var tasks = await this.todoTaskService.GetAllByListIdAsync(
                        listId.Value,
                        userId.Value,
                        filter,
                        pageNumber: viewModel.TaskPageNumber,
                        rowCount: viewModel.TaskRowCount);

                    TodoListsLog.LogTasksRetrievedForList(this.logger, tasks.Count, listId.Value, userId.Value);

                    // Map tasks to view models
                    viewModel.Tasks = tasks.Select(t =>
                    {
                        var taskViewModel = MapToViewModel.ToTask(userId.Value, t);
                        taskViewModel.Role = Formaters.StringToRoleEnum(selectedList.UserRole);
                        return taskViewModel;
                    }).ToList();
                }
            }

            return this.View(viewModel);
        }
        catch (Exception ex)
        {
            TodoListsLog.LogFailedToLoadTodoLists(this.logger, ex);
            throw;
        }
    }

    [HttpGet]
    public IActionResult Create(Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        var userId = UserHelper.GetCurrentUserId(this.User);
        if (userId == null)
        {
            return this.RedirectToAction("Login", "Account");
        }

        TodoListsLog.LogCreatePageLoaded(this.logger, userId.Value);

        if (returnUrl == null)
        {
            returnUrl = new Uri(this.Url.Action("Index", "TodoLists") ?? "~/", UriKind.Relative);
        }

        var viewModel = new CreateTodoListViewModel
        {
            ReturnUrl = returnUrl,
        };

        return this.View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Create(CreateTodoListViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (!this.ModelState.IsValid)
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

            var list = await this.todoListService.GetByIdAsync(userId.Value, id);
            if (list == null)
            {
                TodoListsLog.LogListNotFoundForEdit(this.logger, id, userId.Value);
                return this.NotFound();
            }

            TodoListsLog.LogListLoadedForEdit(this.logger, id, userId.Value);

            if (returnUrl == null)
            {
                returnUrl = new Uri(this.Url.Action("Index", "TodoLists") ?? "~/", UriKind.Relative);
            }

            var viewModel = new EditTodoListViewModel
            {
                ListId = list.Id,
                Title = list.Title,
                Description = list.Description,
                UserRole = list.UserRole,
                PendingTasks = list.ActiveTasks,
                ReturnUrl = returnUrl,
            };

            return this.View(viewModel);
        }
        catch (Exception ex)
        {
            TodoListsLog.LogErrorLoadingEditPage(this.logger, id, ex);
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(EditTodoListViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (!this.ModelState.IsValid)
        {
            return this.EditInternal(model);
        }

        return Task.FromResult<IActionResult>(this.View(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            await this.todoListService.DeleteAsync(userId.Value, id);

            TodoListsLog.LogListDeletedSuccessfully(this.logger, id, userId.Value);

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.RedirectToAction(nameof(this.Index));
        }
        catch (Exception ex)
        {
            TodoListsLog.LogErrorDeletingList(this.logger, id, ex);
            throw;
        }
    }

    private async Task<IActionResult> CreateInternal(CreateTodoListViewModel model)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var listModel = new TodoListModel(
                id: 0,
                ownerId: userId.Value,
                title: model.Title,
                description: model.Description ?? string.Empty);

            var createdList = await this.todoListService.AddAsync(listModel);

            TodoListsLog.LogListCreatedSuccessfully(this.logger, createdList.Id, userId.Value);

            return this.Redirect(model.ReturnUrl.ToString());
        }
        catch (Exception ex)
        {
            TodoListsLog.LogFailedToLoadCreateTodoList(this.logger, ex);
            this.ModelState.AddModelError(string.Empty, "An error occurred while creating the list.");
            throw;
        }
    }

    private async Task<IActionResult> EditInternal(EditTodoListViewModel model)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var listModel = new TodoListModel(
                id: model.ListId,
                ownerId: userId.Value,
                title: model.Title,
                description: model.Description ?? string.Empty);

            var updatedList = await this.todoListService.UpdateAsync(userId.Value, listModel);

            TodoListsLog.LogListUpdatedSuccessfully(this.logger, updatedList.Id, userId.Value);

            return this.Redirect(model.ReturnUrl.ToString());
        }
        catch (Exception ex)
        {
            TodoListsLog.LogErrorUpdatingList(this.logger, model.ListId, ex);
            this.ModelState.AddModelError(string.Empty, "An error occurred while updating the list.");
            return this.View(model);
            throw;
        }
    }
}
