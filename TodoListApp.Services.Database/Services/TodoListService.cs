using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Repositories;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Services;

/// <summary>
/// Service for managing to-do lists.
/// </summary>
public class TodoListService : ITodoListService
{
    private readonly TodoListRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListService"/> class.
    /// </summary>
    /// <param name="context">The to-do list database context.</param>
    public TodoListService(TodoListDbContext context)
    {
        this.repository = new TodoListRepository(context);
    }

    /// <summary>
    /// Adds a new to-do list.
    /// </summary>
    /// <param name="model">The model containing info about the new list.</param>
    /// <returns>The created to-do list model.</returns>
    /// <exception cref="EntityWithIdExistsException">If the model contains the id that is already in the context.</exception>
    /// <exception cref="UnableToCreateException">If the service is unable to add the list.</exception>
    public async Task<TodoListModel> AddAsync(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        TodoList? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is not null)
        {
            throw new EntityWithIdExistsException(nameof(TodoList), model.Id);
        }

        try
        {
            TodoList newList = await this.repository.AddAsync(ModelToEntity(model));
            return EntityToModel(newList, model.OwnerId);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToCreateException(nameof(TodoList), ex, model.Id);
        }
    }

    /// <summary>
    /// Deletes a to-do list by its id.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="id">The unique identifier of the list.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="EntityNotFoundException">If there is no entity with the specified id, or user is not the list owner.</exception>
    /// <exception cref="UnableToDeleteException">If the service is unable to delte the list.</exception>
    public async Task DeleteAsync(int userId, int id)
    {
        TodoList? existing = await this.repository.GetByIdAsync(id);
        if (existing is null || existing.OwnerId != userId)
        {
            throw new EntityNotFoundException(nameof(existing), id);
        }

        try
        {
            bool succes = await this.repository.DeleteByIdAsync(id);
            if (!succes)
            {
                throw new UnableToDeleteException(nameof(existing), id);
            }
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToDeleteException(nameof(existing), id, ex);
        }
    }

    /// <summary>
    /// Gets all to-do lists created by a specific author.
    /// </summary>
    /// <param name="authorId">The unique identifier of the user.</param>
    /// <returns>A read-only list of to-do list models.</returns>
    public async Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId)
    {
        var todoLists = await this.repository.GetAllByAuthorAsync(authorId);
        return todoLists
            .Select(l =>
            EntityToModel(l, authorId))
            .ToList();
    }

    /// <summary>
    /// Gets a paginated list of to-do lists created by a specific author.
    /// </summary>
    /// <param name="authorId">The unique identifier of the user.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the list.</param>
    /// <returns>A read-only list of to-do list models.</returns>
    public async Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId, int pageNumber, int rowCount)
    {
        var todoLists = await this.repository.GetAllByAuthorAsync(authorId, pageNumber, rowCount);
        return todoLists
            .Select(l =>
            EntityToModel(l, authorId))
            .ToList();
    }

    /// <summary>
    /// Gets all to-do lists a user has access to (either as owner or collaborator).
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A read-only list of to-do list models.</returns>
    public async Task<IReadOnlyList<TodoListModel>> GetAllAsync(int userId)
    {
        var todoLists = await this.repository.GetAllByUserAsync(userId);
        return todoLists
            .Select(l =>
            EntityToModel(l, userId))
            .ToList();
    }

    /// <summary>
    /// Gets a paginated list of to-do lists a user has access to (either as owner or collaborator).
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the list.</param>
    /// <returns>A read-only list of to-do list models.</returns>
    public async Task<IReadOnlyList<TodoListModel>> GetAllAsync(int userId, int pageNumber, int rowCount)
    {
        var todoLists = await this.repository.GetAllByUserAsync(userId, pageNumber, rowCount);
        return todoLists
            .Select(l =>
            EntityToModel(l, userId))
            .ToList();
    }

    /// <summary>
    /// Gets a to-do list by its id.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="id">The list unique identifier.</param>
    /// <returns>The to-do list model.</returns>
    /// <exception cref="EntityNotFoundException">If there is no entity with the specified ID in the context.</exception>
    /// <exception cref="UnauthorizedAccessException">If the user has no access to the list.</exception>
    public async Task<TodoListModel> GetByIdAsync(int userId, int id)
    {
        var entity = await this.repository.GetByIdAsync(id);

        if (entity == null)
        {
            throw new EntityNotFoundException(nameof(entity), id);
        }
        else if (!entity.TodoListUserRoles.Any(lur => lur.UserId == userId && lur.ListRole.RoleName == "Editor") && entity.OwnerId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to see this list.");
        }

        return EntityToModel(entity, userId);
    }

    /// <summary>
    /// Updates an existing to-do list.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="model">The model with the new info about the list.</param>
    /// <returns>The updated to-do list model.</returns>
    /// <exception cref="EntityNotFoundException">If there is no entity with the specified ID in the context.</exception>
    /// <exception cref="UnauthorizedAccessException">If the user has no rights to eddit the list.</exception>
    /// <exception cref="UnableToUpdateException">If service is unable to update the list.</exception>
    public async Task<TodoListModel> UpdateAsync(int userId, TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        TodoList? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(existing), model.Id);
        }
        else if (!existing.TodoListUserRoles.Any(lur => lur.UserId == userId && lur.ListRole.RoleName == "Editor") && existing.OwnerId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to see this list.");
        }

        try
        {
            TodoList? updated = await this.repository.UpdateAsync(ModelToEntity(model));
            return updated == null ?
                throw new UnableToUpdateException(nameof(existing), model.Id) :
                EntityToModel(updated, userId);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(existing), model.Id, ex);
        }
    }

    private static TodoListModel EntityToModel(TodoList entity, int userId)
    {
        string? userRole = null;

        if (entity.OwnerId == userId)
        {
            userRole = "Owner";
        }
        else
        {
            var role = entity.TodoListUserRoles.FirstOrDefault(r => r.UserId == userId);
            if (role != null)
            {
                userRole = role.ListRole.RoleName;
            }
            else
            {
                userRole = "No role";
            }
        }

        var activeTasks = 0;
        if (entity.TodoTasks != null)
        {
            activeTasks = (from task in entity.TodoTasks
                           where task.Status != null && task.Status.StatusTitle != "Completed"
                           select task).Count();
        }

        string userLastName = string.Empty;
        if (entity.ListOwner != null && !string.IsNullOrEmpty(entity.ListOwner.LastName))
        {
            userLastName = $" {entity.ListOwner.LastName[0]}.";
        }

        var userFullName = (entity.ListOwner is null) ?
            null :
            string.Concat(entity.ListOwner.FirstName, userLastName);

        return new TodoListModel(entity.Id, entity.OwnerId, entity.Title, entity.Description, activeTasks, userFullName, userRole);
    }

    private static TodoList ModelToEntity(TodoListModel model)
    {
        var entity = new TodoList()
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description ?? string.Empty,
            OwnerId = model.OwnerId,
        };

        return entity;
    }
}
