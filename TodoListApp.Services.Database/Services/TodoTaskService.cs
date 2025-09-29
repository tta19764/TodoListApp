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
public class TodoTaskService : ITodoTaskService
{
    private readonly TodoTaskRepository repository;

    public TodoTaskService(TodoListDbContext context)
    {
        this.repository = new TodoTaskRepository(context);
    }

    public async Task<TodoTaskModel> AddAsync(TodoTaskModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        model.CreationDate = null;

        var entity = await this.repository.AddAsync(ModelToEntity(model));
        return EntityToModel(entity, model.OwnerUserId);
    }

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

    public async Task<IReadOnlyList<TodoTaskModel>> GetAllAsync(int userId)
    {
        var lists = await this.repository.GetAllUserTasksAsync(userId);
        return lists
            .Select(l =>
            EntityToModel(l, userId))
            .ToList();
    }

    public async Task<IReadOnlyList<TodoTaskModel>> GetAllAsync(int userId, int pageNumber, int rowCount)
    {
        var lists = await this.repository.GetAllUserTasksAsync(userId, pageNumber, rowCount);
        return lists
            .Select(l =>
            EntityToModel(l, userId))
            .ToList();
    }

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
