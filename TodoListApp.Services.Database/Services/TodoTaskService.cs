using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Repositories;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Services;

/// <summary>
/// Service for managing to-do tasks.
/// </summary>
public class TodoTaskService : ITodoTaskService
{
    private readonly TodoTaskRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTaskService"/> class.
    /// </summary>
    /// <param name="context">The to-do lists database context.</param>
    public TodoTaskService(TodoListDbContext context)
    {
        this.repository = new TodoTaskRepository(context);
    }

    /// <summary>
    /// Adds a new to-do task.
    /// </summary>
    /// <param name="model">The model containing the info about the new task.</param>
    /// <returns>The added to-do task model.</returns>
    /// <exception cref="ArgumentNullException">If the model is null.</exception>
    /// <exception cref="EntityWithIdExistsException">If a task with the same ID already exists.</exception>
    public async Task<TodoTaskModel> AddAsync(TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        TodoTask? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is not null)
        {
            throw new EntityWithIdExistsException(nameof(TodoTask), model.Id);
        }

        try
        {
            model.CreationDate = null;
            TodoTask newTask = await this.repository.AddAsync(ModelToEntity(model));
            return EntityToModel(newTask, model.OwnerUserId);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToCreateException(nameof(TodoList), ex, model.Id);
        }
    }

    /// <summary>
    /// Deletes a to-do task by its ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="id">The unique identifier of the task.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    /// <exception cref="EntityNotFoundException">If there is no entity with the specified ID in the context.</exception>
    /// <exception cref="UnauthorizedAccessException">If the user has no permission to delete the task.</exception>
    /// <exception cref="UnableToDeleteException">Is service is unable to delete the task.</exception>
    public async Task DeleteAsync(int userId, int id)
    {
        TodoTask? existing = await this.repository.GetByIdAsync(id);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), id);
        }
        else if (existing.TodoList.OwnerId != userId &&
            !existing.TodoList.TodoListUserRoles.Any(lur => lur.UserId == userId && lur.ListRole.RoleName != "Editor"))
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this task.");
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
    /// Gets all to-do tasks the specified user hass access to.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A read-only list of to-do task models.</returns>
    public async Task<IReadOnlyList<TodoTaskModel>> GetAllAsync(int userId)
    {
        var lists = await this.repository.GetAllUserTasksAsync(userId);
        return lists
            .Select(l =>
            EntityToModel(l, userId))
            .ToList();
    }

    /// <summary>
    /// Gets all to-do tasks the specified user has access to with pagination.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>A read-only list of to-do task models.</returns>
    public async Task<IReadOnlyList<TodoTaskModel>> GetAllAsync(int userId, int pageNumber, int rowCount)
    {
        var lists = await this.repository.GetAllUserTasksAsync(userId, pageNumber, rowCount);
        return lists
            .Select(l =>
            EntityToModel(l, userId))
            .ToList();
    }

    /// <summary>
    /// Gets a to-do task by its ID if the specified user has access to it.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="id">The unique identifier of the task.</param>
    /// <returns>The to-do task model.</returns>
    /// <exception cref="EntityNotFoundException">If there is no entity with the specified ID in the context.</exception>
    /// <exception cref="UnauthorizedAccessException">If the user has no rights to see the task.</exception>
    public async Task<TodoTaskModel> GetByIdAsync(int userId, int id)
    {
        var entity = await this.repository.GetByIdAsync(id);

        if (entity is null)
        {
            throw new EntityNotFoundException(nameof(entity), id);
        }
        else if (entity.TodoList.OwnerId != userId &&
            !entity.TodoList.TodoListUserRoles.Any(lur => lur.UserId == userId))
        {
            throw new UnauthorizedAccessException("You do not have permission to see this task.");
        }

        return EntityToModel(entity, userId);
    }

    /// <summary>
    /// Updates an existing to-do task.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="model">The model containing the new info about the task.</param>
    /// <returns>The updated to-do task model.</returns>
    /// <exception cref="EntityNotFoundException">If there is no task with the specified ID in the context.</exception>
    /// <exception cref="UnauthorizedAccessException">If the user has no rights to edit the task.</exception>
    /// <exception cref="UnableToUpdateException">If the service is unable to update the task.</exception>
    public async Task<TodoTaskModel> UpdateAsync(int userId, TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        TodoTask? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), model.Id);
        }
        else if (existing.TodoList.OwnerId != userId &&
            !existing.TodoList.TodoListUserRoles.Any(lur => lur.UserId == userId && lur.ListRole.RoleName != "Editor"))
        {
            throw new UnauthorizedAccessException("You do not have permission to update this task.");
        }

        try
        {
            model.CreationDate = existing.CreationDate;
            TodoTask? updated = await this.repository.UpdateAsync(ModelToEntity(model));
            return updated is null ?
                throw new UnableToUpdateException(nameof(existing), model.Id) :
                EntityToModel(updated, userId);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(existing), model.Id, ex);
        }
    }

    /// <summary>
    /// Gets all to-do tasks by the list ID with filtering, sorting, and pagination.
    /// </summary>
    /// <param name="id">The unqiue identifier of the list.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="filter">The filtering by task status.</param>
    /// <param name="sortBy">The sorting property.</param>
    /// <param name="sortOrder">The sortyng direction.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>A read-only list of to-do task models.</returns>
    public async Task<IReadOnlyList<TodoTaskModel>> GetAllByListIdAsync(int id, int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null)
    {
        sortOrder = string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase) ? sortOrder : "desc";
        sortBy = string.Equals(sortOrder, "Title", StringComparison.OrdinalIgnoreCase) ? sortBy : "DueDate";

        IReadOnlyList<TodoTask>? tasks;

        if (pageNumber != null && rowCount != null)
        {
            int page = pageNumber > 0 ? (int)pageNumber : 1;
            int row = rowCount > 0 ? (int)rowCount : 1;

            tasks = await this.repository.GetAllByListIdAsync(id, page, row);
        }
        else
        {
            tasks = await this.repository.GetAllByListIdAsync(id);
        }

        var filteredTasks = filter switch
        {
            TaskFilter.InProgress => tasks.Where(t => t.StatusId == 2),
            TaskFilter.NotStarted => tasks.Where(t => t.StatusId == 1),
            TaskFilter.Completed => tasks.Where(t => t.StatusId == 3),
            TaskFilter.Active => tasks.Where(t => t.StatusId != 3),
            TaskFilter.All => tasks,
            _ => tasks
        };

        var sortedTasks = ApplySorting(filteredTasks.AsQueryable(), sortBy!, sortOrder!);
        return sortedTasks.Select(t => EntityToModel(t, userId)).ToList();
    }

    /// <summary>
    /// Gets all to-do tasks created by the specified user with filtering, sorting, and pagination.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="filter">The filtering by task status.</param>
    /// <param name="sortBy">The sorting property.</param>
    /// <param name="sortOrder">The sortyng direction.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>A read-only list of to-do task models.</returns>
    public async Task<IReadOnlyList<TodoTaskModel>> GetAllByAuthorAsync(int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null)
    {
        sortOrder = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase) ? sortOrder : "asc";
        sortBy = string.Equals(sortBy, "Title", StringComparison.OrdinalIgnoreCase) ? sortBy : "DueDate";

        IReadOnlyList<TodoTask>? tasks;

        if (pageNumber != null && rowCount != null)
        {
            int page = pageNumber > 0 ? (int)pageNumber : 1;
            int row = rowCount > 0 ? (int)rowCount : 1;

            tasks = await this.repository.GetAllUserTasksAsync(userId, page, row);
        }
        else
        {
            tasks = await this.repository.GetAllUserTasksAsync(userId);
        }

        var filteredTasks = filter switch
        {
            TaskFilter.InProgress => tasks.Where(t => t.StatusId == 2),
            TaskFilter.NotStarted => tasks.Where(t => t.StatusId == 1),
            TaskFilter.Completed => tasks.Where(t => t.StatusId == 3),
            TaskFilter.Active => tasks.Where(t => t.StatusId != 3),
            TaskFilter.All => tasks,
            _ => tasks
        };

        var sortedTasks = ApplySorting(filteredTasks.AsQueryable(), sortBy!, sortOrder!);
        return sortedTasks.Select(t => EntityToModel(t, userId)).ToList();
    }

    /// <summary>
    /// Updates the status of a to-do task.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="taskId">The unique identifier of the task.</param>
    /// <param name="statusId">The unique identifier of the status.</param>
    /// <returns>The updated to-do task model.</returns>
    /// <exception cref="EntityNotFoundException">If there is not task with the specifed ID in the context.</exception>
    /// <exception cref="UnauthorizedAccessException">If the use has no rights to edit the task.</exception>
    /// <exception cref="UnableToUpdateException">If the service is unable to update the task.</exception>
    public async Task<TodoTaskModel> UpdateTaskStatusAsync(int userId, int taskId, int statusId)
    {
        TodoTask? existing = await this.repository.GetByIdAsync(taskId);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), taskId);
        }
        else if (existing.TodoList.OwnerId != userId &&
            !existing.TodoList.TodoListUserRoles.Any(lur => lur.UserId == userId && lur.ListRole.RoleName != "Editor"))
        {
            throw new UnauthorizedAccessException("You do not have permission to update this task.");
        }

        try
        {
            existing.StatusId = statusId;
            var model = EntityToModel(existing, userId);
            TodoTask? updated = await this.repository.UpdateAsync(ModelToEntity(model));
            return updated is null ?
                throw new UnableToUpdateException(nameof(existing), model.Id) :
                EntityToModel(updated, userId);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(existing), taskId, ex);
        }
    }

    private static IQueryable<TodoTask> ApplySorting(IQueryable<TodoTask> query, string sortBy, string sortOrder)
    {
        var isAsc = string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase);

        return sortBy.ToUpperInvariant() switch
        {
            "TITLE" => isAsc
                ? query.OrderBy(t => t.Title)
                : query.OrderByDescending(t => t.Title),

            "DUEDATE" => isAsc
                ? query.OrderBy(t => t.DueDate)
                : query.OrderByDescending(t => t.DueDate),

            _ => isAsc
                ? query.OrderBy(t => t.Id)
                : query.OrderByDescending(t => t.Id)
        };
    }

    private static TodoTask ModelToEntity(TodoTaskModel model)
    {
        var entity = new TodoTask()
        {
            Id = model.Id,
            ListId = model.ListId,
            DueDate = model.DueDate,
            Description = model.Description,
            CreationDate = model.CreationDate ?? DateTime.UtcNow,
            OwnerUserId = model.OwnerUserId,
            StatusId = model.StatusId,
            Title = model.Title,
        };

        return entity;
    }

    private static TodoTaskModel EntityToModel(TodoTask entity, int userId)
    {
        var tasktags = new List<TagModel>();
        if (entity.TaskTags != null)
        {
            foreach (var taskTag in entity.TaskTags)
            {
                if (taskTag.Tag.UserId == userId)
                {
                    tasktags.Add(new TagModel(taskTag.TagId, taskTag.Tag.Label, taskTag.Tag.UserId, taskTag.TaskId));
                }
            }
        }

        var taskCommetns = new List<CommentModel>();
        if (entity.Comments != null)
        {
            foreach (var taskComment in entity.Comments)
            {
                if (taskComment.UserId == userId)
                {
                    taskCommetns.Add(new CommentModel(taskComment.Text, taskComment.TaskId, taskComment.UserId));
                }
            }
        }

        var model = new TodoTaskModel(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.CreationDate,
            entity.DueDate,
            entity.StatusId,
            entity.OwnerUserId,
            entity.ListId,
            new UserModel(entity.OwnerUserId, entity.OwnerUser.FirstName, entity.OwnerUser.LastName),
            new StatusModel(entity.StatusId, entity.Status.StatusTitle),
            new ReadOnlyCollection<TagModel>(tasktags),
            new ReadOnlyCollection<CommentModel>(taskCommetns));

        return model;
    }
}
