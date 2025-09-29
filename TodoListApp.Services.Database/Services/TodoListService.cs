using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Repositories;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Services;
public class TodoListService : ITodoListService
{
    private readonly TodoListRepository repository;

    public TodoListService(TodoListDbContext context)
    {
        this.repository = new TodoListRepository(context);
    }

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

    public async Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId)
    {
        var todoLists = await this.repository.GetAllByAuthorAsync(authorId);
        return todoLists
            .Select(l =>
            EntityToModel(l, authorId))
            .ToList();
    }

    public async Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId, int pageNumber, int rowCount)
    {
        var todoLists = await this.repository.GetAllByAuthorAsync(authorId, pageNumber, rowCount);
        return todoLists
            .Select(l =>
            EntityToModel(l, authorId))
            .ToList();
    }

    public async Task<IReadOnlyList<TodoListModel>> GetAllAsync(int userId)
    {
        var todoLists = await this.repository.GetAllByUserAsync(userId);
        return todoLists
            .Select(l =>
            EntityToModel(l, userId))
            .ToList();
    }

    public async Task<IReadOnlyList<TodoListModel>> GetAllAsync(int userId, int pageNumber, int rowCount)
    {
        var todoLists = await this.repository.GetAllByUserAsync(userId, pageNumber, rowCount);
        return todoLists
            .Select(l =>
            EntityToModel(l, userId))
            .ToList();
    }

    public async Task<TodoListModel> GetByIdAsync(int userId, int id)
    {
        var entity = await this.repository.GetByIdAsync(id);

        if (entity == null)
        {
            throw new EntityNotFoundException(nameof(entity), id);
        }
        else if (entity.TodoListUserRoles.Any(lur => lur.UserId != userId))
        {
            throw new UnauthorizedAccessException("You do not have permission to see this task.");
        }

        return EntityToModel(entity, userId);
    }

    public async Task<TodoListModel> UpdateAsync(int userId, TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        TodoList? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is null || existing.OwnerId != userId)
        {
            throw new EntityNotFoundException(nameof(existing), model.Id);
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
        TodoListUserRoleModel? userRole = null;

        if (entity.OwnerId != userId)
        {
            var role = entity.TodoListUserRoles.FirstOrDefault(r => r.UserId == userId);
            if (role != null)
            {
                userRole = new TodoListUserRoleModel(role.Id, role.TodoListRoleId, userId, role.ListRole.RoleName);
            }
            else
            {
                userRole = new TodoListUserRoleModel(0, 0, userId, "Unknown");
            }
        }

        var tasks = new List<TodoTaskModel>();
        if (entity.TodoTasks != null)
        {
            foreach (var task in entity.TodoTasks)
            {
                UserModel? ownerUser = null;

                if (task.OwnerUser != null)
                {
                    ownerUser = new UserModel(task.OwnerUserId, task.OwnerUser.FirstName, task.OwnerUser.LastName);
                }

                StatusModel? statusModel = null;

                if (task.Status != null)
                {
                    statusModel = new StatusModel(task.StatusId, task.Status.StatusTitle);
                }

                ReadOnlyCollection<TagModel>? tagModels = null;
                if (task.TaskTags != null)
                {
                    var tags = new List<TagModel>();
                    foreach (var tag in task.TaskTags)
                    {
                        tags.Add(new TagModel(tag.Tag.Id, tag.Tag.Label, tag.TaskId, tag.Tag.UserId));
                    }

                    tagModels = new ReadOnlyCollection<TagModel>(tags);
                }

                tasks.Add(new TodoTaskModel(task.Id, task.Title, task.Description, task.CreationDate, task.DueDate, task.StatusId, task.OwnerUserId, task.ListId, ownerUser, statusModel, tagModels));
            }
        }

        var userModel = (entity.ListOwner is null) ? null : new UserModel(entity.OwnerId, entity.ListOwner.FirstName, entity.ListOwner.LastName);

        return new TodoListModel(entity.Id, entity.OwnerId, entity.Title, entity.Description, userModel, userRole, new ReadOnlyCollection<TodoTaskModel>(tasks));
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
