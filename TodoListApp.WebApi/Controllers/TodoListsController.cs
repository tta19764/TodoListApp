using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;
using TodoListApp.WebApi.CustomLogs;
using TodoListApp.WebApi.Models.Dtos.Create;
using TodoListApp.WebApi.Models.Dtos.Read;
using TodoListApp.WebApi.Models.Dtos.Update;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// An API controller for managing to-do lists, including operations to create, retrieve, update, and delete lists.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodoListsController : ControllerBase
{
    private const string EmptyName = "N/A";
    private readonly ITodoListService service;
    private readonly ILogger<TodoListsController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListsController"/> class.
    /// </summary>
    /// <param name="todoListService">The service to retrieve lists from database.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="todoListService"/> or <paramref name="logger"/> is null.</exception>
    public TodoListsController(ITodoListService todoListService, ILogger<TodoListsController> logger)
    {
        this.service = todoListService ?? throw new ArgumentNullException(nameof(todoListService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves a to-do list by its ID.
    /// </summary>
    /// <param name="listId">The unique identifier of the list.</param>
    /// <returns>The to-do list if found; otherwise, an appropriate error response.</returns>
    [Authorize]
    [HttpGet("{listId:int}")]
    public async Task<ActionResult<TodoListDto>> GetList(int listId)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                TodoListsLog.LogInvalidUserIdentifier(this.logger, userId);
                return this.Unauthorized("Invalid user identifier.");
            }

            var todoList = await this.service.GetByIdAsync(userId.Value, listId);

            if (todoList == null)
            {
                TodoListsLog.LogListNotFoundForUser(this.logger, listId, userId, "List not found");
                return this.NotFound("The list was not found.");
            }

            var result = MapToDto(todoList);
            TodoListsLog.LogListRetrievedSuccessfully(this.logger, listId, userId);
            return this.Ok(result);
        }
        catch (EntityNotFoundException ex)
        {
            TodoListsLog.LogListNotFoundForUser(this.logger, listId, this.GetCurrentUserId(), ex.Message);
            return this.NotFound($"List with ID {listId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoListsLog.LogUnauthorizedListAccess(this.logger, listId, this.GetCurrentUserId(), ex.Message);
            return this.Forbid("You don't have permission to access this list.");
        }
        catch (Exception ex)
        {
            TodoListsLog.LogUnexpectedErrorRetrievingList(this.logger, listId, ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all to-do lists accessible to the current user.
    /// </summary>
    /// <returns>A list of to-do lists if found; otherwise, an appropriate error response.</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<TodoListDto>>> GetLists()
    {
        try
        {
            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                TodoListsLog.LogInvalidUserIdentifier(this.logger, userId);
                return this.Unauthorized("Invalid user identifier.");
            }

            var todoLists = await this.service.GetAllAsync(userId.Value);
            var results = todoLists.Select(MapToDto).ToList();

            TodoListsLog.LogListsRetrievedSuccessfully(this.logger, results.Count, userId);
            return this.Ok(results);
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoListsLog.LogUnauthorizedListsAccess(this.logger, this.GetCurrentUserId(), ex.Message);
            return this.Forbid("You don't have permission to access these lists.");
        }
        catch (Exception ex)
        {
            TodoListsLog.LogUnexpectedErrorRetrievingLists(this.logger, this.GetCurrentUserId(), ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a paginated list of to-do lists accessible to the current user.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of lists on the page.</param>
    /// <returns>A paginated list of to-do lists if found; otherwise, an appropriate error response.</returns>
    [Authorize]
    [HttpGet("{pageNumber:min(1)}/{rowCount:min(1)}")]
    public async Task<ActionResult<List<TodoListDto>>> GetListsPaginated(int pageNumber, int rowCount)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                TodoListsLog.LogInvalidUserIdentifier(this.logger, userId);
                return this.Unauthorized("Invalid user identifier.");
            }

            var todoLists = await this.service.GetAllAsync(userId.Value, pageNumber, rowCount);
            var results = todoLists.Select(MapToDto).ToList();

            TodoListsLog.LogPaginatedListsRetrievedSuccessfully(this.logger, results.Count, pageNumber, rowCount, userId);
            return this.Ok(results);
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoListsLog.LogUnauthorizedListsAccess(this.logger, this.GetCurrentUserId(), ex.Message);
            return this.Forbid("You don't have permission to access these lists.");
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "pageNumber" || ex.ParamName == "rowCount")
        {
            TodoListsLog.LogInvalidPaginationParameters(this.logger, pageNumber, rowCount);
            return this.BadRequest("Invalid pagination parameters. Page number and row count must be positive integers.");
        }
        catch (Exception ex)
        {
            TodoListsLog.LogUnexpectedErrorRetrievingPaginatedLists(this.logger, this.GetCurrentUserId(), ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all to-do lists created by the current user.
    /// </summary>
    /// <returns>A list of to-do lists if found; otherwise, an appropriate error response.</returns>
    [Authorize]
    [HttpGet("UserLists")]
    public async Task<ActionResult<List<TodoListDto>>> GetUsersLists()
    {
        try
        {
            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                TodoListsLog.LogInvalidUserIdentifier(this.logger, userId);
                return this.Unauthorized("Invalid user identifier.");
            }

            var todoLists = await this.service.GetAllByAuthorAsync(userId.Value);
            var results = todoLists.Select(MapToDto).ToList();

            TodoListsLog.LogListsRetrievedSuccessfully(this.logger, results.Count, userId);
            return this.Ok(results);
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoListsLog.LogUnauthorizedListsAccess(this.logger, this.GetCurrentUserId(), ex.Message);
            return this.Forbid("You don't have permission to access these lists.");
        }
        catch (Exception ex)
        {
            TodoListsLog.LogUnexpectedErrorRetrievingUserLists(this.logger, this.GetCurrentUserId(), ex);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a paginated list of to-do lists created by the current user.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of lists on the page.</param>
    /// <returns>A paginated list of to-do lists if found; otherwise, an appropriate error response.</returns>
    [Authorize]
    [HttpGet("UserLists/{pageNumber:min(1)}/{rowCount:min(1)}")]
    public async Task<ActionResult<List<TodoListDto>>> GetUsersListsPaginated(int pageNumber, int rowCount)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                TodoListsLog.LogInvalidUserIdentifier(this.logger, userId);
                return this.Unauthorized("Invalid user identifier.");
            }

            var todoLists = await this.service.GetAllByAuthorAsync(userId.Value, pageNumber, rowCount);
            var results = todoLists.Select(MapToDto).ToList();

            TodoListsLog.LogPaginatedListsRetrievedSuccessfully(this.logger, results.Count, pageNumber, rowCount, userId);
            return this.Ok(results);
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoListsLog.LogUnauthorizedListsAccess(this.logger, this.GetCurrentUserId(), ex.Message);
            return this.Forbid("You don't have permission to access these lists.");
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "pageNumber" || ex.ParamName == "rowCount")
        {
            TodoListsLog.LogInvalidPaginationParameters(this.logger, pageNumber, rowCount);
            return this.BadRequest("Invalid pagination parameters. Page number and row count must be positive integers.");
        }
        catch (Exception ex)
        {
            TodoListsLog.LogUnexpectedErrorRetrievingPaginatedUserLists(this.logger, this.GetCurrentUserId(), ex);
            throw;
        }
    }

    /// <summary>
    /// Deletes a to-do list by its ID.
    /// </summary>
    /// <param name="listId">The unique identifier of the list to delete.</param>
    /// <returns>An appropriate response indicating the result of the deletion operation.</returns>
    [Authorize]
    [HttpDelete("{listId:int}")]
    public async Task<ActionResult> DeleteuserList(int listId)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                TodoListsLog.LogInvalidUserIdentifier(this.logger, userId);
                return this.Unauthorized("Invalid user identifier.");
            }

            await this.service.DeleteAsync(userId.Value, listId);

            TodoListsLog.LogListDeletedSuccessfully(this.logger, listId, userId);
            var response = new { message = "The list has been deleted successfully." };
            return this.Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            TodoListsLog.LogListNotFoundForUser(this.logger, listId, this.GetCurrentUserId(), ex.Message);
            return this.NotFound($"List with ID {listId} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoListsLog.LogUnauthorizedListDeletion(this.logger, listId, this.GetCurrentUserId(), ex.Message);
            return this.Forbid("You don't have permission to delete this list.");
        }
        catch (UnableToDeleteException ex)
        {
            TodoListsLog.LogUnableToDeleteList(this.logger, listId, this.GetCurrentUserId(), ex.Message, ex);
            return this.StatusCode(500, "Unable to delete the list. Please try again later.");
        }
        catch (Exception ex)
        {
            TodoListsLog.LogUnexpectedErrorDeletingList(this.logger, listId, ex);
            throw;
        }
    }

    /// <summary>
    /// Creates a new to-do list.
    /// </summary>
    /// <param name="dto">The data transfer object that contains the info for the new list.</param>
    /// <returns>The created to-do list if successful; otherwise, an appropriate error response.</returns>
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<TodoListDto>> CreateList([FromBody] CreateTodoListDto dto)
    {
        try
        {
            if (dto == null)
            {
                TodoListsLog.LogInvalidListDataProvided(this.logger, "DTO is null");
                return this.BadRequest("List data is required.");
            }

            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                TodoListsLog.LogInvalidUserIdentifier(this.logger, userId);
                return this.Unauthorized("Invalid user identifier.");
            }

            var model = new TodoListModel(0, userId.Value, dto.Title, dto.Description);
            var newEntity = await this.service.AddAsync(model);

            TodoListsLog.LogListCreatedSuccessfully(this.logger, newEntity.Id, userId);

            var result = MapToDto(newEntity);
            return this.CreatedAtAction(nameof(this.GetList), new { listId = newEntity.Id }, result);
        }
        catch (ArgumentNullException ex)
        {
            TodoListsLog.LogInvalidListDataProvided(this.logger, ex.Message);
            return this.BadRequest("Invalid list data provided.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoListsLog.LogUnauthorizedListsAccess(this.logger, this.GetCurrentUserId(), ex.Message);
            return this.Forbid("You don't have permission to create lists.");
        }
        catch (Exception ex)
        {
            TodoListsLog.LogUnexpectedErrorCreatingList(this.logger, ex);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing to-do list.
    /// </summary>
    /// <param name="dto">The data transfer object that contains the updated info of the list.</param>
    /// <returns>The updated to-do list if successful; otherwise, an appropriate error response.</returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TodoListDto>> UpadateList([FromBody] UpdateTodoListDto dto)
    {
        if (dto == null)
        {
            TodoListsLog.LogInvalidListDataProvided(this.logger, "DTO is null");
            return this.BadRequest("List data is required.");
        }

        try
        {
            var userId = this.GetCurrentUserId();
            if (userId == null)
            {
                TodoListsLog.LogInvalidUserIdentifier(this.logger, userId);
                return this.Unauthorized("Invalid user identifier.");
            }

            var model = new TodoListModel(dto.Id, userId.Value, dto.Title, dto.Description);
            var updatedEntity = await this.service.UpdateAsync(userId.Value, model);

            TodoListsLog.LogListUpdatedSuccessfully(this.logger, dto.Id, userId);

            var result = MapToDto(updatedEntity);
            return this.Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            TodoListsLog.LogInvalidListDataProvided(this.logger, ex.Message);
            return this.BadRequest("Invalid list data provided.");
        }
        catch (EntityNotFoundException ex)
        {
            TodoListsLog.LogListNotFoundForUser(this.logger, dto.Id, this.GetCurrentUserId(), ex.Message);
            return this.NotFound($"List with ID {dto.Id} was not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            TodoListsLog.LogUnauthorizedListUpdate(this.logger, dto.Id, this.GetCurrentUserId(), ex.Message);
            return this.Forbid("You don't have permission to update this list.");
        }
        catch (UnableToUpdateException ex)
        {
            TodoListsLog.LogUnableToUpdateList(this.logger, dto.Id, this.GetCurrentUserId(), ex.Message, ex);
            return this.StatusCode(500, "Unable to update the list. Please try again later.");
        }
        catch (Exception ex)
        {
            TodoListsLog.LogUnexpectedErrorUpdatingList(this.logger, dto.Id, ex);
            throw;
        }
    }

    private static TodoListDto MapToDto(TodoListModel model)
    {
        return new TodoListDto(
            model.Id,
            model.Title,
            model.Description,
            model.OwnerFullName ?? EmptyName,
            model.UserRole,
            model.OwnerId,
            model.ActiveTasks);
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
