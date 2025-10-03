using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.WebApp.CustomLogs;
using TodoListApp.WebApp.Models.List;
using TodoListApp.WebApp.Models.Task;

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
    public async Task<IActionResult> Index(int? listId, string? listFilter, string? taskFilter)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var viewModel = new TodoListsPageViewModel
            {
                SelectedListId = listId,
                ListFilter = listFilter ?? "all",
                TaskFilter = taskFilter ?? "active",
            };

            IReadOnlyList<TodoListModel> lists;

            if (listFilter == "owned")
            {
                lists = await this.todoListService.GetAllByAuthorAsync(userId.Value);
            }
            else if (listFilter == "shared")
            {
                var allLists = await this.todoListService.GetAllAsync(userId.Value);
                lists = allLists.Where(l => l.OwnerId != userId.Value).ToList();
            }
            else
            {
                lists = await this.todoListService.GetAllAsync(userId.Value);
            }

            TodoListsLog.LogListsRetrievedForUser(this.logger, lists.Count, userId.Value);

            // Map to view models
            viewModel.TodoLists = lists.Select(l => MapToListViewModel(l)).AsEnumerable();

            // If a list is selected, load its tasks
            if (listId.HasValue)
            {
                // Get the selected list details
                var selectedList = lists.FirstOrDefault(l => l.Id == listId.Value);
                if (selectedList != null)
                {
                    viewModel.SelectedList = MapToListViewModel(selectedList);

                    // Parse task filter
                    var filter = ParseTaskFilter(taskFilter);

                    // Load tasks
                    var tasks = await this.todoTaskService.GetAllByListIdAsync(
                        listId.Value,
                        userId.Value,
                        filter);

                    TodoListsLog.LogTasksRetrievedForList(this.logger, tasks.Count, listId.Value, userId.Value);

                    // Map tasks to view models
                    viewModel.Tasks = tasks.Select(t => MapToTaskViewModel(t)).ToList();
                }
            }

            return this.View(viewModel);
        }
        catch (Exception ex)
        {
            TodoListsLog.LogFailedToLoadTodoLists(this.logger, ex);
            return this.View("Error");
            throw;
        }
    }

    [HttpGet]
    public IActionResult Create(Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        var userId = this.GetCurrentUserId();
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
    public async Task<IActionResult> Create(CreateTodoListViewModel model)
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
            return this.View("Error");
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditTodoListViewModel model)
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = this.GetCurrentUserId();
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
            return this.RedirectToAction(nameof(this.Index));
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

    private static TodoListViewModel MapToListViewModel(TodoListModel model)
    {
        return new TodoListViewModel
        {
            ListId = model.Id,
            Title = model.Title,
            Description = model.Description,
            UserRole = model.UserRole ?? "Unknown",
            PendingTasks = model.ActiveTasks,
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
            Tags = model.UsersTags?.Select(tag => tag.StatusTitle) ?? Enumerable.Empty<string>(),
            Comments = model.UserComments?.Select(c => c.Text) ?? Enumerable.Empty<string>(),
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
