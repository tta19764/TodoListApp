using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.WebApi.CustomLogs;
using TodoListApp.WebApi.Helpers;
using TodoListApp.WebApi.Models.Dtos.Create;
using TodoListApp.WebApi.Models.Dtos.Read;
using TodoListApp.WebApi.Models.Dtos.Update;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for managing to-do tasks.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodoTasksController : ControllerBase
{
    private readonly ITodoTaskService service;
    private readonly ILogger<TodoTasksController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTasksController"/> class.
    /// </summary>
    /// <param name="service">The service for managing to-do tasks.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown when a required parameter is null.</exception>
    public TodoTasksController(
        ITodoTaskService service,
        ILogger<TodoTasksController> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the count of comments for a specific task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <returns>The count of comments.</returns>
    [HttpGet("{taskId:int}/Comments/Count")]
    [Authorize]
    public async Task<ActionResult<int>> GetTaskCommentsCount(int taskId)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var count = await this.service.GetTaskCommentsCountAsync(taskId, userId.Value);

            return this.Ok(count);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Task with ID {taskId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTaskAccess(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to access this task.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingComments(this.logger, taskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving comments count.");
            throw;
        }
    }

    /// <summary>
    /// Gets all comments for a specific task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <returns>A list of comments.</returns>
    [HttpGet("{taskId:int}/Comments")]
    [Authorize]
    public async Task<ActionResult<List<CommentDto>>> GetTaskComments(int taskId)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var comments = await this.service.GetTaskCommentsAsync(taskId, userId.Value);

            if (comments == null || !comments.Any())
            {
                return this.Ok(new List<CommentDto>());
            }

            var commentDtos = comments.Select(MapToDto.ToCommentDto).ToList();

            TodoTasksLog.LogCommentsRetrievedForTask(this.logger, commentDtos.Count, taskId);

            return this.Ok(commentDtos);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Task with ID {taskId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTaskAccess(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to access this task.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingComments(this.logger, taskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving comments.");
            throw;
        }
    }

    /// <summary>
    /// Gets paginated comments for a specific task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of comments per page.</param>
    /// <returns>A paginated list of comments.</returns>
    [HttpGet("{taskId:int}/Comments/{pageNumber:min(1)}/{rowCount:min(1)}")]
    [Authorize]
    public async Task<ActionResult<List<CommentDto>>> GetTaskCommentsPaginated(
        int taskId,
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

            var comments = await this.service.GetTaskCommentsAsync(taskId, userId.Value, pageNumber, rowCount);

            if (comments == null || !comments.Any())
            {
                return this.Ok(new List<CommentDto>());
            }

            var commentDtos = comments.Select(MapToDto.ToCommentDto).ToList();

            TodoTasksLog.LogCommentsRetrievedForTask(this.logger, commentDtos.Count, taskId);

            return this.Ok(commentDtos);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Task with ID {taskId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTaskAccess(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to access this task.");
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "pageNumber" || ex.ParamName == "rowCount")
        {
            TodoTasksLog.LogInvalidPaginationParameters(this.logger, pageNumber, rowCount);
            return this.BadRequest("Invalid pagination parameters. Page number and row count must be positive integers.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingComments(this.logger, taskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving comments.");
            throw;
        }
    }

    /// <summary>
    /// Adds a comment to a task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <param name="dto">The data transfer object containing comment information.</param>
    /// <returns>The created comment.</returns>
    [HttpPost("{taskId:int}/Comments")]
    [Authorize]
    public async Task<ActionResult<CommentDto>> AddCommentToTask(int taskId, [FromBody] CreateCommentDto dto)
    {
        try
        {
            if (dto == null)
            {
                return this.BadRequest("Comment data is required.");
            }

            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var commentModel = new CommentModel(0, dto.Text, taskId, userId.Value);
            var createdComment = await this.service.AddTaskCommentAsync(commentModel);

            TodoTasksLog.LogCommentAddedToTask(this.logger, createdComment.Id, taskId, userId);

            var commentDto = MapToDto.ToCommentDto(createdComment);

            return this.CreatedAtAction(nameof(this.GetTaskComments), new { taskId }, commentDto);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Task with ID {taskId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedCommentOperation(this.logger, taskId, 0, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to add comments to this task.");
        }
        catch (UnableToCreateException ex)
        {
            TodoTasksLog.LogUnexpectedErrorAddingComment(this.logger, taskId, ex);
            return this.StatusCode(500, "Unable to add comment. Please try again later.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorAddingComment(this.logger, taskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while adding comment.");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <param name="dto">The data transfer object containing updated comment information.</param>
    /// <returns>The updated comment.</returns>
    [HttpPut("{taskId:int}/Comments/{commentId:int}")]
    [Authorize]
    public async Task<ActionResult<CommentDto>> UpdateComment(int taskId, [FromBody] UpdateCommentDto dto)
    {
        if (dto == null)
        {
            return this.BadRequest("Comment data is required.");
        }

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var commentModel = new CommentModel(dto.Id, dto.Text, taskId, userId.Value);
            var updatedComment = await this.service.UpdateTaskCommentAsync(userId.Value, commentModel);

            TodoTasksLog.LogCommentUpdated(this.logger, dto.Id, userId);

            return this.Ok(MapToDto.ToCommentDto(updatedComment));
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogCommentNotFound(this.logger, dto.Id, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Comment with ID {dto.Id} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedCommentOperation(this.logger, taskId, dto.Id, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to update this comment.");
        }
        catch (UnableToUpdateException ex)
        {
            TodoTasksLog.LogUnexpectedErrorUpdatingComment(this.logger, dto.Id, ex);
            return this.StatusCode(500, "Unable to update comment. Please try again later.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorUpdatingComment(this.logger, dto.Id, ex);
            return this.StatusCode(500, "An unexpected error occurred while updating comment.");
            throw;
        }
    }

    /// <summary>
    /// Deletes a comment from a task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns>The result of the deletion operation.</returns>
    [HttpDelete("{taskId:int}/Comments/{commentId:int}")]
    [Authorize]
    public async Task<ActionResult> DeleteComment(int taskId, int commentId)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            await this.service.RemoveTaskCommentAsync(userId.Value, taskId, commentId);

            TodoTasksLog.LogCommentDeleted(this.logger, commentId, userId);

            return this.Ok(new { message = "Comment has been deleted successfully." });
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogCommentNotFound(this.logger, commentId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Comment with ID {commentId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedCommentOperation(this.logger, taskId, commentId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to delete this comment.");
        }
        catch (UnableToDeleteException ex)
        {
            TodoTasksLog.LogUnexpectedErrorDeletingComment(this.logger, commentId, ex);
            return this.StatusCode(500, "Unable to delete comment. Please try again later.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorDeletingComment(this.logger, commentId, ex);
            return this.StatusCode(500, "An unexpected error occurred while deleting comment.");
            throw;
        }
    }

    /// <summary>
    /// Gets a specific comment by its ID.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <param name="commnetId">The comment ID.</param>
    /// <returns>The comment with the specified ID if the user has access to it.</returns>
    [HttpGet("{taskId:int}/Comments/{commnetId:int}")]
    [Authorize]
    public async Task<ActionResult<CommentDto>> GetComment(int taskId, int commnetId)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var model = await this.service.GetCommentByIdAsync(userId.Value, taskId, commnetId);

            if (model == null)
            {
                return this.NotFound($"Comment with ID {commnetId} was not found.");
            }

            return this.Ok(MapToDto.ToCommentDto(model));
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogCommentNotFound(this.logger, commnetId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Comment with ID {commnetId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTaskAccess(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to access this comment.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingTask(this.logger, taskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving the comment.");
            throw;
        }
    }

    /// <summary>
    /// Maps a TodoTask model to a TodoTaskDto.
    /// </summary>
    /// <param name="listId">The list unique identifier.</param>
    /// <param name="filter">The task status filter.</param>
    /// <returns>Count of tasks in the list.</returns>
    [HttpGet("Lists/{listId:int}/Count")]
    [Authorize]
    public async Task<ActionResult<int>> GetListTasksCount(
        int listId,
        [FromQuery] TaskFilter filter = TaskFilter.Active)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var count = await this.service.GetAllByListIdCountAsync(listId, userId.Value, filter);

            return this.Ok(count);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogListNotFoundForUser(this.logger, listId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"List with ID {listId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedListAccess(this.logger, listId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to access this list.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingListTasks(this.logger, listId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tasks.");
            throw;
        }
    }

    /// <summary>
    /// Gets all tasks for a specific list.
    /// </summary>
    /// <param name="listId">The unique identifier of the list.</param>
    /// <param name="filter">The status type to filter the list.</param>
    /// <param name="sortBy">The property to sort the list.</param>
    /// <param name="sortOrder">The sorting order.</param>
    /// <returns>A list of tasks for a specified lidt if the user has access to it.</returns>
    [HttpGet("Lists/{listId:int}")]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> GetListTasks(
        int listId,
        [FromQuery] TaskFilter filter = TaskFilter.Active,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var tasks = await this.service.GetAllByListIdAsync(listId, userId.Value, filter, sortBy, sortOrder);

            if (tasks == null || !tasks.Any())
            {
                return this.Ok(new List<TodoTaskDto>());
            }

            var taskDtos = tasks.Select(MapToDto.ToTodoTaskDto).ToList();
            return this.Ok(taskDtos);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogListNotFoundForUser(this.logger, listId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"List with ID {listId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedListAccess(this.logger, listId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to access this list.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingListTasks(this.logger, listId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tasks.");
            throw;
        }
    }

    /// <summary>
    /// Gets paginated tasks for a specific list.
    /// </summary>
    /// <param name="listId">The unique identifier of the list.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks in the page.</param>
    /// <param name="filter">The status type to filter the list.</param>
    /// <param name="sortBy">The property to sort the list.</param>
    /// <param name="sortOrder">The sorting order.</param>
    /// <returns>A paginated list of tasks for a specified list if the user has access to it.</returns>
    [HttpGet("Lists/{listId:int}/{pageNumber:min(1)}/{rowCount:min(1)}")]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> GetListTasksPaginated(
        int listId,
        int pageNumber,
        int rowCount,
        [FromQuery] TaskFilter filter = TaskFilter.Active,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var tasks = await this.service.GetAllByListIdAsync(listId, userId.Value, filter, sortBy, sortOrder, pageNumber, rowCount);

            if (tasks == null || !tasks.Any())
            {
                return this.Ok(new List<TodoTaskDto>());
            }

            var taskDtos = tasks.Select(MapToDto.ToTodoTaskDto).ToList();
            return this.Ok(taskDtos);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogListNotFoundForUser(this.logger, listId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"List with ID {listId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedListAccess(this.logger, listId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to access this list.");
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "pageNumber" || ex.ParamName == "rowCount")
        {
            TodoTasksLog.LogInvalidPaginationParameters(this.logger, pageNumber, rowCount);
            return this.BadRequest("Invalid pagination parameters. Page number and row count must be positive integers.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingPaginatedListTasks(this.logger, listId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tasks.");
            throw;
        }
    }

    /// <summary>
    /// Gets all tasks the cyrrent user has access to.
    /// </summary>
    /// <returns>A list of tasks assigned to the current user.</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> GetTasks()
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var tasks = await this.service.GetAllAsync(userId.Value);

            if (tasks == null || !tasks.Any())
            {
                return this.Ok(new List<TodoTaskDto>());
            }

            var taskDtos = tasks.Select(MapToDto.ToTodoTaskDto).ToList();
            return this.Ok(taskDtos);
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTasksAccess(this.logger, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to access these tasks.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingUserTasks(this.logger, UserHelper.GetCurrentUserId(this.User), ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tasks.");
            throw;
        }
    }

    /// <summary>
    /// Gets all tasks the cyrrent user has access to.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks in the page.</param>
    /// <returns>A paginated list of tasks assigned to the current user.</returns>
    [HttpGet("{pageNumber:min(1)}/{rowCount:min(1)}")]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> GetTasksPaginated(
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

            var tasks = await this.service.GetAllAsync(userId.Value, pageNumber, rowCount);

            if (tasks == null || !tasks.Any())
            {
                return this.Ok(new List<TodoTaskDto>());
            }

            var taskDtos = tasks.Select(MapToDto.ToTodoTaskDto).ToList();
            return this.Ok(taskDtos);
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTasksAccess(this.logger, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to access these tasks.");
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "pageNumber" || ex.ParamName == "rowCount")
        {
            TodoTasksLog.LogInvalidPaginationParameters(this.logger, pageNumber, rowCount);
            return this.BadRequest("Invalid pagination parameters. Page number and row count must be positive integers.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingPaginatedUserTasks(this.logger, UserHelper.GetCurrentUserId(this.User), ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving tasks.");
            throw;
        }
    }

    /// <summary>
    /// Gets a specific task by its ID.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <returns>The task with the specified ID if the user has access to it.</returns>
    [HttpGet("{taskId:int}")]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> GetTask(int taskId)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var model = await this.service.GetByIdAsync(userId.Value, taskId);

            if (model == null)
            {
                return this.NotFound($"Task with ID {taskId} was not found.");
            }

            var taskDto = MapToDto.ToTodoTaskDto(model);
            return this.Ok(taskDto);
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Task with ID {taskId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTaskAccess(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to access this task.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorRetrievingTask(this.logger, taskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while retrieving the task.");
            throw;
        }
    }

    /// <summary>
    /// Deletes a specific task by its ID.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <returns>The result of the deletion operation.</returns>
    [HttpDelete("{taskId:int}")]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> DeleteTask(int taskId)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            await this.service.DeleteAsync(userId.Value, taskId);
            TodoTasksLog.LogTaskDeletedSuccessfully(this.logger, taskId, userId);

            return this.Ok(new { message = "The task has been deleted successfully." });
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTaskNotFoundForUser(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Task with ID {taskId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTaskDeletion(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to delete this task.");
        }
        catch (UnableToDeleteException ex)
        {
            TodoTasksLog.LogUnableToDeleteTask(this.logger, taskId, UserHelper.GetCurrentUserId(this.User), ex.Message, ex);
            return this.StatusCode(500, "Unable to delete the task. Please try again later.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorDeletingTask(this.logger, taskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while deleting the task.");
            throw;
        }
    }

    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="dto">The data transfer object containing the info about the task.</param>
    /// <returns>The created task.</returns>
    [HttpPut]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> CreateTask([FromBody] CreateTodoTaskDto dto)
    {
        try
        {
            if (dto == null)
            {
                return this.BadRequest("Task data is required.");
            }

            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var model = new TodoTaskModel(0, dto.Title, dto.Description, null, dto.DueDate, dto.StatusId, dto.AssigneeId, dto.ListId);
            var createdTask = await this.service.AddAsync(model);

            TodoTasksLog.LogTaskCreatedSuccessfully(this.logger, createdTask.Id, userId);

            var taskDto = MapToDto.ToTodoTaskDto(createdTask);
            return this.CreatedAtAction(nameof(this.GetTask), new { taskId = createdTask.Id }, taskDto);
        }
        catch (ArgumentNullException ex)
        {
            TodoTasksLog.LogInvalidTaskDataProvided(this.logger, ex.Message);
            return this.BadRequest("Invalid task data provided.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTasksAccess(this.logger, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to create tasks in this list.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorCreatingTask(this.logger, ex);
            return this.StatusCode(500, "An unexpected error occurred while creating the task.");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="dto">The data transfer object containing the info about the task.</param>
    /// <returns>The updated task.</returns>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TodoTaskDto>> UpdateTask([FromBody] UpdateTodoTaskDto dto)
    {
        if (dto == null)
        {
            return this.BadRequest("Task data is required.");
        }

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var model = new TodoTaskModel(dto.Id, dto.Title, dto.Description, null, dto.DueDate, dto.StatusId, dto.AssigneeId, dto.ListId);
            var updatedTask = await this.service.UpdateAsync(userId.Value, model);

            TodoTasksLog.LogTaskUpdatedSuccessfully(this.logger, dto.Id, userId);

            var taskDto = MapToDto.ToTodoTaskDto(updatedTask);
            return this.Ok(taskDto);
        }
        catch (ArgumentNullException ex)
        {
            TodoTasksLog.LogInvalidTaskDataProvided(this.logger, ex.Message);
            return this.BadRequest("Invalid task data provided.");
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTaskNotFoundForUser(this.logger, dto.Id, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Task with ID {dto.Id} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTaskUpdate(this.logger, dto.Id, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to update this task.");
        }
        catch (UnableToUpdateException ex)
        {
            TodoTasksLog.LogUnableToUpdateTask(this.logger, dto.Id, UserHelper.GetCurrentUserId(this.User), ex.Message, ex);
            return this.StatusCode(500, "Unable to update the task. Please try again later.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorUpdatingTask(this.logger, dto.Id, ex);
            return this.StatusCode(500, "An unexpected error occurred while updating the task.");
            throw;
        }
    }

    /// <summary>
    /// Updates the status of an existing task.
    /// </summary>
    /// <param name="dto">The data transfer object containing the ID of the new status and the ID of an existing task.</param>
    /// <returns>The updated task.</returns>
    [HttpPatch]
    [Authorize]
    public async Task<ActionResult<TodoTaskDto>> UpdateTaskStatus([FromBody] UpdateStatusDto dto)
    {
        if (dto == null)
        {
            return this.BadRequest("Task data is required.");
        }

        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var updatedTask = await this.service.UpdateTaskStatusAsync(userId.Value, dto.TaskId, dto.StatusId);

            TodoTasksLog.LogTaskUpdatedSuccessfully(this.logger, dto.StatusId, userId);

            var taskDto = MapToDto.ToTodoTaskDto(updatedTask);
            return this.Ok(taskDto);
        }
        catch (ArgumentNullException ex)
        {
            TodoTasksLog.LogInvalidTaskDataProvided(this.logger, ex.Message);
            return this.BadRequest("Invalid task data provided.");
        }
        catch (EntityNotFoundException ex)
        {
            TodoTasksLog.LogTaskNotFoundForUser(this.logger, dto.TaskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.NotFound($"Task with ID {dto.TaskId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoTasksLog.LogUnauthorizedTaskUpdate(this.logger, dto.TaskId, UserHelper.GetCurrentUserId(this.User), ex.Message);
            return this.Forbid("You don't have permission to update this task.");
        }
        catch (UnableToUpdateException ex)
        {
            TodoTasksLog.LogUnableToUpdateTask(this.logger, dto.TaskId, UserHelper.GetCurrentUserId(this.User), ex.Message, ex);
            return this.StatusCode(500, "Unable to update the task. Please try again later.");
        }
        catch (Exception ex)
        {
            TodoTasksLog.LogUnexpectedErrorUpdatingTask(this.logger, dto.TaskId, ex);
            return this.StatusCode(500, "An unexpected error occurred while updating the task.");
            throw;
        }
    }
}
