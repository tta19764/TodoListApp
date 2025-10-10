using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.WebApp.CustomLogs;
using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models.Tag;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Controller for managing tags and their association with tasks.
/// </summary>
[Authorize]
public class TagsController : Controller
{
    private readonly ITaskTagService taskTagService;
    private readonly ITagService tagService;
    private readonly ILogger<TagsController> logger;

    public TagsController(
        ITaskTagService taskTagService,
        ITagService tagService,
        ILogger<TagsController> logger)
    {
        this.taskTagService = taskTagService ?? throw new ArgumentNullException(nameof(taskTagService));
        this.tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// View all tags created by the user.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A view displaying the list of tags.</returns>
    [HttpGet]
    public async Task<IActionResult> Index(int? pageNumber, int? rowCount)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var viewModel = new AllTagsViewModel
            {
                PageNumber = pageNumber ?? 1,
                RowCount = rowCount ?? 20,
            };

            // Get total count
            var totalCount = await this.taskTagService.GetAllUserTaskTagsCount(userId.Value);
            viewModel.TotalPages = (int)Math.Ceiling(totalCount / (double)viewModel.RowCount);
            viewModel.TotalTags = totalCount;

            // Get all tags for the user
            var tags = await this.taskTagService.GetAllUserTaskTagsAsync(
                userId.Value,
                viewModel.PageNumber,
                viewModel.RowCount);

            viewModel.Tags = tags.Select(MapToViewModel.ToTag).ToList();

            TagsLog.LogTagsRetrievedForUser(
                this.logger,
                viewModel.Tags.Count(),
                userId.Value);

            return this.View(viewModel);
        }
        catch (Exception ex)
        {
            TagsLog.LogErrorLoadingTagsList(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// View all tasks associated with a specific tag.
    /// </summary>
    /// <param name="tagId">The tag ID.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A view displaying the list of tasks associated with the tag.</returns>
    [HttpGet]
    public async Task<IActionResult> TasksByTag(int tagId, int? pageNumber, int? rowCount, Uri? returnUri)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var viewModel = new TagTasksViewModel
            {
                TagId = tagId,
                PageNumber = pageNumber ?? 1,
                RowCount = rowCount ?? 10,
                ReturnUrl = returnUri ?? new Uri(this.Url.Action("Index", "Tags") ?? "~/", UriKind.RelativeOrAbsolute),
            };

            // Get the tag details
            var tag = await this.tagService.GetByIdAsync(tagId);
            if (tag == null)
            {
                TagsLog.LogTagNotFound(this.logger, tagId);
                return this.NotFound();
            }

            viewModel.TagTitle = tag.Title;

            // Get total count
            var totalCount = await this.taskTagService.GetTagTasksCount(userId.Value, tagId);
            viewModel.TotalPages = (int)Math.Ceiling(totalCount / (double)viewModel.RowCount);
            viewModel.TotalTasks = totalCount;

            // Get all tasks with this tag
            var tasks = await this.taskTagService.GetAllUserTagTasksAsync(
                userId.Value,
                tagId,
                viewModel.PageNumber,
                viewModel.RowCount);

            viewModel.Tasks = tasks.Select(t => MapToViewModel.ToTask(userId.Value, t)).ToList();

            TagsLog.LogTasksRetrievedForTag(
                this.logger,
                tagId,
                viewModel.Tasks.Count(),
                userId.Value);

            return this.View(viewModel);
        }
        catch (Exception ex)
        {
            TagsLog.LogErrorLoadingTasksForTag(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Adds a tag to a task.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <param name="returnUrl">The return Url.</param>
    /// <returns>A view displaying the form to add a tag to the task.</returns>
    [HttpGet]
    public async Task<IActionResult> AddTag(int taskId, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var viewModel = new AddTagToTaskViewModel
            {
                TaskId = taskId,
                ReturnUrl = returnUrl ?? new Uri(this.Url.Action("Details", "TodoTasks", new { id = taskId }) ?? "~/", UriKind.RelativeOrAbsolute),
            };

            // Get available tags for this task (tags not already added)
            var availableTags = await this.tagService.GetAvailableTaskTagsAsync(taskId);
            viewModel.AvailableTags = availableTags.Select(MapToViewModel.ToTag).ToList();

            if (!viewModel.AvailableTags.Any())
            {
                this.TempData["ErrorMessage"] = "No available tags to add. All tags have been added to this task.";
                return this.Redirect(viewModel.ReturnUrl.ToString());
            }

            TagsLog.LogTagsRetrievedForUser(
                this.logger,
                viewModel.AvailableTags.Count(),
                userId.Value);

            return this.View(viewModel);
        }
        catch (Exception ex)
        {
            TagsLog.LogErrorLoadingAddTagPage(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Handles the submission of the form to add a tag to a task.
    /// </summary>
    /// <param name="model">The new tag model.</param>
    /// <returns>A redirect to the return URL or the form view in case of an error.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTag(AddTagToTaskViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        _ = this.ModelState.IsValid;

        if (!model.SelectedTagId.HasValue)
        {
            this.ModelState.AddModelError(nameof(model.SelectedTagId), "Please select a tag");

            // Reload available tags
            var availableTags = await this.tagService.GetAvailableTaskTagsAsync(model.TaskId);
            model.AvailableTags = availableTags.Select(t => new TagViewModel
            {
                Id = t.Id,
                Title = t.Title,
            }).ToList();

            return this.View(model);
        }

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var updatedTask = await this.taskTagService.AddTaskTag(userId.Value, model.TaskId, model.SelectedTagId.Value);

            TagsLog.LogTagAddedToTask(
                this.logger,
                model.SelectedTagId.Value,
                updatedTask.Id,
                userId.Value);

            this.TempData["SuccessMessage"] = "Tag added successfully";

            return this.Redirect(model.ReturnUrl.ToString());
        }
        catch (Exception ex)
        {
            TagsLog.LogErrorAddingTagToTask(this.logger, ex);

            this.ModelState.AddModelError(string.Empty, "An error occurred while adding the tag");

            // Reload available tags
            var availableTags = await this.tagService.GetAvailableTaskTagsAsync(model.TaskId);
            model.AvailableTags = availableTags.Select(MapToViewModel.ToTag).ToList();
            throw;
        }
    }

    /// <summary>
    /// Removes a tag from a task.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <param name="tagId">The Tag ID.</param>
    /// <param name="returnUrl">The return Url.</param>
    /// <returns>A redirect to the return URL or the task details page in case of an error.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveTag(int taskId, int tagId, Uri? returnUrl)
    {
        _ = this.ModelState.IsValid;

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            var updatedTask = await this.taskTagService.RemoveTaskTag(userId.Value, taskId, tagId);

            TagsLog.LogTagRemovedFromTask(
                this.logger,
                tagId,
                updatedTask.Id,
                userId.Value);

            this.TempData["SuccessMessage"] = "Tag removed successfully";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.RedirectToAction("Details", "TodoTasks", new { id = taskId });
        }
        catch (Exception ex)
        {
            TagsLog.LogErrorRemovingTagFromTask(this.logger, ex);

            this.TempData["ErrorMessage"] = "An error occurred while removing the tag";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.RedirectToAction("Details", "TodoTasks", new { id = taskId });
            throw;
        }
    }
}
