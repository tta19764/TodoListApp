using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.WebApi.CustomLogs;
using TodoListApp.WebApi.Helpers;
using TodoListApp.WebApi.Models.Dtos.Read;

namespace TodoListApp.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TaskTagsController : ControllerBase
{
    private readonly ITaskTagService service;
    private readonly ILogger<TaskTagsController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskTagsController"/> class.
    /// </summary>
    /// <param name="service">The service for managing to-do tasks.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown when a required parameter is null.</exception>
    public TaskTagsController(
        ITaskTagService service,
        ILogger<TaskTagsController> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the count of all tags for the current user.
    /// </summary>
    /// <returns>The count of tags.</returns>
    [HttpGet("Count")]
    [Authorize]
    public async Task<ActionResult<int>> GetUserTagsCount()
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var count = await this.service.GetAllUserTaskTagsCount(userId.Value);

            return this.Ok(count);
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingTags(this.logger, UserHelper.GetCurrentUserId(this.User), ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tags count.");
            throw;
        }
    }

    /// <summary>
    /// Gets all tags for the current user.
    /// </summary>
    /// <returns>A list of tags.</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<TagDto>>> GetUserTags()
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var tags = await this.service.GetAllUserTaskTagsAsync(userId.Value);

            if (tags == null || !tags.Any())
            {
                return this.Ok(new List<TagDto>());
            }

            var tagDtos = tags.Select(t => new TagDto { Id = t.Id, Title = t.Title }).ToList();

            TodoTasksLog.LogTagsRetrievedSuccessfully(this.logger, tagDtos.Count, userId);

            return this.Ok(tagDtos);
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingTags(this.logger, UserHelper.GetCurrentUserId(this.User), ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tags.");
            throw;
        }
    }

    /// <summary>
    /// Gets paginated tags for the current user.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tags per page.</param>
    /// <returns>A paginated list of tags.</returns>
    [HttpGet("{pageNumber:min(1)}/{rowCount:min(1)}")]
    [Authorize]
    public async Task<ActionResult<List<TagDto>>> GetUserTagsPaginated(
        int pageNumber,
        int rowCount)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var tags = await this.service.GetAllUserTaskTagsAsync(userId.Value, pageNumber, rowCount);

            if (tags == null || !tags.Any())
            {
                return this.Ok(new List<TagDto>());
            }

            var tagDtos = tags.Select(t => new TagDto { Id = t.Id, Title = t.Title }).ToList();

            TodoTasksLog.LogTagsRetrievedSuccessfully(this.logger, tagDtos.Count, userId);

            return this.Ok(tagDtos);
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "pageNumber" || ex.ParamName == "rowCount")
        {
            TodoTasksLog.LogInvalidPaginationParameters(this.logger, pageNumber, rowCount);
            return this.BadRequest("Invalid pagination parameters. Page number and row count must be positive integers.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingTags(this.logger, UserHelper.GetCurrentUserId(this.User), ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tags.");
            throw;
        }
    }

    /// <summary>
    /// Gets the count of tasks tagged with a specific tag.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>The count of tasks with the specified tag.</returns>
    [HttpGet("{tagId:int}/Tasks/Count")]
    [Authorize]
    public async Task<ActionResult<int>> GetTagTasksCount(int tagId)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var count = await this.service.GetTagTasksCount(userId.Value, tagId);

            return this.Ok(count);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTagNotFoundForUser(this.logger, tagId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Tag with ID {tagId} was not found.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingTaggedTasks(this.logger, tagId, UserHelper.GetCurrentUserId(this.User), ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tagged tasks count.");
            throw;
        }
    }

    /// <summary>
    /// Gets all tasks tagged with a specific tag.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>A list of tasks with the specified tag.</returns>
    [HttpGet("{tagId:int}/Tasks")]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> GetTagTasks(int tagId)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var tasks = await this.service.GetAllUserTagTasksAsync(userId.Value, tagId);

            if (tasks == null || !tasks.Any())
            {
                return this.Ok(new List<TodoTaskDto>());
            }

            var taskDtos = tasks.Select(MapToDto.ToTodoTaskDto).ToList();

            TodoTasksLog.LogTaggedTasksRetrievedSuccessfully(this.logger, taskDtos.Count, tagId, userId);

            return this.Ok(taskDtos);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTagNotFoundForUser(this.logger, tagId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Tag with ID {tagId} was not found.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingTaggedTasks(this.logger, tagId, UserHelper.GetCurrentUserId(this.User), ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tagged tasks.");
            throw;
        }
    }

    /// <summary>
    /// Gets paginated tasks tagged with a specific tag.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks per page.</param>
    /// <returns>A paginated list of tasks with the specified tag.</returns>
    [HttpGet("{tagId:int}/Tasks/{pageNumber:min(1)}/{rowCount:min(1)}")]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> GetTagTasksPaginated(
        int tagId,
        int pageNumber,
        int rowCount)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var tasks = await this.service.GetAllUserTagTasksAsync(userId.Value, tagId, pageNumber, rowCount);

            if (tasks == null || !tasks.Any())
            {
                return this.Ok(new List<TodoTaskDto>());
            }

            var taskDtos = tasks.Select(MapToDto.ToTodoTaskDto).ToList();

            TodoTasksLog.LogTaggedTasksRetrievedSuccessfully(this.logger, taskDtos.Count, tagId, userId);

            return this.Ok(taskDtos);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTagNotFoundForUser(this.logger, tagId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Tag with ID {tagId} was not found.");
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "pageNumber" || ex.ParamName == "rowCount")
        {
            TodoTasksLog.LogInvalidPaginationParameters(this.logger, pageNumber, rowCount);
            return this.BadRequest("Invalid pagination parameters. Page number and row count must be positive integers.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingTaggedTasks(this.logger, tagId, UserHelper.GetCurrentUserId(this.User), ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tagged tasks.");
            throw;
        }
    }

    /// <summary>
    /// Adds a tag to a task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>The updated task with the new tag.</returns>
    [HttpPost("Tasks/{taskId:int}/Tags/{tagId:int}")]
    [Authorize]
    public async Task<ActionResult<TodoTaskDto>> AddTagToTask(int taskId, int tagId)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var updatedTask = await this.service.AddTaskTag(userId.Value, taskId, tagId);

            TodoTasksLog.LogTagAddedToTask(this.logger, taskId, tagId, userId);

            var taskDto = MapToDto.ToTodoTaskDto(updatedTask);
            return this.Ok(taskDto);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Task with ID {taskId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTagOperation(this.logger, taskId, tagId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to add tags to this task.");
        }
        catch (UnableToUpdateException ex)
        {
            TodoTasksLog.LogUnexpectedErrorAddingTag(this.logger, taskId, tagId, ex);
            return this.StatusCode(500, "Unable to add tag to task. Please try again later.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorAddingTag(this.logger, taskId, tagId, ex);
            return this.StatusCode(500, "An unexpected error occurred while adding tag to task.");
            throw;
        }
    }

    /// <summary>
    /// Removes a tag from a task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>The updated task without the removed tag.</returns>
    [HttpDelete("Tasks/{taskId:int}/Tags/{tagId:int}")]
    [Authorize]
    public async Task<ActionResult<TodoTaskDto>> RemoveTagFromTask(int taskId, int tagId)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var updatedTask = await this.service.RemoveTaskTag(userId.Value, taskId, tagId);

            TodoTasksLog.LogTagRemovedFromTask(this.logger, taskId, tagId, userId);

            var taskDto = MapToDto.ToTodoTaskDto(updatedTask);
            return this.Ok(taskDto);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Task with ID {taskId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTagOperation(this.logger, taskId, tagId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to remove tags from this task.");
        }
        catch (UnableToUpdateException ex)
        {
            TodoTasksLog.LogUnexpectedErrorRemovingTag(this.logger, taskId, tagId, ex);
            return this.StatusCode(500, "Unable to remove tag from task. Please try again later.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRemovingTag(this.logger, taskId, tagId, ex);
            return this.StatusCode(500, "An unexpected error occurred while removing tag from task.");
            throw;
        }
    }
}
