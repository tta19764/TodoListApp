using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.WebApi.CustomLogs;
using TodoListApp.WebApi.Helpers;
using TodoListApp.WebApi.Models.Dtos.Read;

namespace TodoListApp.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SearchTasksController : ControllerBase
{
    private readonly ISearchTasksService service;
    private readonly ILogger<SearchTasksController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTasksController"/> class.
    /// </summary>
    /// <param name="service">The service for managing to-do tasks.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown when a required parameter is null.</exception>
    public SearchTasksController(
        ISearchTasksService service,
        ILogger<SearchTasksController> logger)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the count of tasks based on optional search criteria: title, creation date, and due date.
    /// </summary>
    /// <param name="title">The task title.</param>
    /// <param name="creationDate">The task creation date.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <returns>The count of tasks matching the search criteria.</returns>
    [HttpGet("Count")]
    [Authorize]
    public async Task<ActionResult<int>> SearchTasksCount(
        [FromQuery] string? title = null,
        [FromQuery] DateTime? creationDate = null,
        [FromQuery] DateTime? dueDate = null)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var count = await this.service.GetAllSearchCountAsync(userId.Value, title, creationDate, dueDate);

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
    /// Searches for tasks based on optional criteria: title, creation date, and due date.
    /// </summary>
    /// <param name="title">The task title.</param>
    /// <param name="creationDate">The task creation date.</param>
    /// <param name="dueDate">Teh task due date.</param>
    /// <returns>A list of tasks matching the search criteria.</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> SearchTasks(
        [FromQuery] string? title = null,
        [FromQuery] DateTime? creationDate = null,
        [FromQuery] DateTime? dueDate = null)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var tasks = await this.service.SearchTasksAsync(userId.Value, title, creationDate, dueDate);

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
    /// Searches for tasks based on optional criteria: title, creation date, and due date with pagination.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">Teh number of tasks on a page.</param>
    /// <param name="title">The task title.</param>
    /// <param name="creationDate">The task creation date.</param>
    /// <param name="dueDate">Teh task due date.</param>
    /// <returns>A paginated list of tasks matching the search criteria.</returns>
    [HttpGet("{pageNumber:min(1)}/{rowCount:min(1)}")]
    [Authorize]
    public async Task<ActionResult<List<TodoTaskDto>>> SearchPaginated(
        int pageNumber,
        int rowCount,
        [FromQuery] string? title = null,
        [FromQuery] DateTime? creationDate = null,
        [FromQuery] DateTime? dueDate = null)
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(this.User);
            if (userId == null)
            {
                return this.Unauthorized("Invalid user identifier.");
            }

            var tasks = await this.service.SearchTasksAsync(userId.Value, title, creationDate, dueDate, pageNumber, rowCount);

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
