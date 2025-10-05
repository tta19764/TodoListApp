using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.WebApi.CustomLogs;
using TodoListApp.WebApi.Helpers;
using TodoListApp.WebApi.Models.Dtos.Read;

namespace TodoListApp.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AssignedTasksController : ControllerBase
{
    private readonly IAssignedTasksService service;
    private readonly ILogger<AssignedTasksController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignedTasksController"/> class.
    /// </summary>
    /// <param name="service">The service for managing to-do tasks.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown when a required parameter is null.</exception>
    public AssignedTasksController(
        IAssignedTasksService service,
        ILogger<AssignedTasksController> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the count of tasks assigned to the current user.
    /// </summary>
    /// <param name="filter">The task status filter.</param>
    /// <returns>The count of tasks assigned to the current user.</returns>
    [HttpGet("Count")]
    [Authorize]
    public async Task<ActionResult<int>> GetAssignedTasksCount(
        [FromQuery] TaskFilter filter = TaskFilter.Active)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var count = await this.service.GetAllAssignedCountAsync(userId.Value, filter);

            return this.Ok(count);
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
    /// Gets all tasks assigned to the current user.
    /// </summary>
    /// <param name="filter">The status type to filter the list.</param>
    /// <param name="sortBy">The property to sort the list.</param>
    /// <param name="sortOrder">The sorting order.</param>
    /// <returns>A list of tasks assigned to the current user.</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> GetAssignedTasks(
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

            var tasks = await this.service.GetAllAssignedAsync(userId.Value, filter, sortBy, sortOrder);

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
    /// Gets paginated tasks assigned to the current user.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks in the page.</param>
    /// <param name="filter">The status type to filter the list.</param>
    /// <param name="sortBy">The property to sort the list.</param>
    /// <param name="sortOrder">The sorting order.</param>
    /// <returns>A paginated list of tasks assigned to the current user.</returns>
    [HttpGet("{pageNumber:min(1)}/{rowCount:min(1)}")]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> GetAssignedTasksPaginated(
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

            var tasks = await this.service.GetAllAssignedAsync(userId.Value, filter, sortBy, sortOrder, pageNumber, rowCount);

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
}
