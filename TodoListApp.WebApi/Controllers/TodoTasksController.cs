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
