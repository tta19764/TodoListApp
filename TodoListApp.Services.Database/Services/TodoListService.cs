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
            return EntityToModel(newList);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToCreateException(nameof(TodoList), ex, model.Id);
        }
    }

    public async Task DeleteAsync(int id)
    {
        TodoList? existing = await this.repository.GetByIdAsync(id);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), id);
        }

        try
        {
            bool succes = await this.repository.DeleteByIdAsync(id);
            if (!succes)
            {
                throw new UnableToDeleteException(nameof(TodoList), id);
            }
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToDeleteException(nameof(TodoList), id, ex);
        }
    }

    public async Task<IReadOnlyList<TodoListModel>> GetAllAsync()
    {
        var lists = await this.repository.GetAllAsync();
        return lists.Select(l =>
            EntityToModel(l))
            .ToList();
    }

    public async Task<IReadOnlyList<TodoListModel>> GetAllAsync(int pageNumber, int rowCount)
    {
        var lists = await this.repository.GetAllAsync(pageNumber, rowCount);
        return lists.Select(l =>
            EntityToModel(l))
            .ToList();
    }

    public async Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId)
    {
        IReadOnlyList<TodoList> todoLists = await this.repository.GetAllAsync();
        return todoLists.Where(
            l => l.OwnerId == authorId)
            .Select(l =>
            EntityToModel(l))
            .ToList();
    }

    public async Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId, int pageNumber, int rowCount)
    {
        IReadOnlyList<TodoList> todoLists = await this.repository.GetAllAsync();
        return todoLists.Where(
            l => l.OwnerId == authorId)
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .Select(l =>
            EntityToModel(l))
            .ToList();
    }

    public async Task<IReadOnlyList<TodoListModel>> GetAllByUserAsync(int userId)
    {
        IReadOnlyList<TodoList> todoLists = await this.repository.GetAllAsync();
        return todoLists.Where(
            l => l.TodoListUserRoles.Any(tlu => tlu.UserId == userId) ||
            l.OwnerId == userId)
            .Select(l =>
            EntityToModel(l))
            .ToList();
    }

    public async Task<IReadOnlyList<TodoListModel>> GetAllByUserAsync(int userId, int pageNumber, int rowCount)
    {
        IReadOnlyList<TodoList> todoLists = await this.repository.GetAllAsync();
        return todoLists.Where(
            l => l.TodoListUserRoles.Any(tlu => tlu.UserId == userId) ||
            l.OwnerId == userId)
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .Select(l =>
            EntityToModel(l))
            .ToList();
    }

    public async Task<TodoListModel> GetByIdAsync(int id)
    {
        return EntityToModel(await this.repository.GetByIdAsync(id) ?? throw new EntityNotFoundException(nameof(TodoTask), id));
    }

    public async Task<TodoListModel> UpdateAsync(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        TodoList? existing = await this.repository.GetByIdAsync(model.Id);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(TodoList), model.Id);
        }

        try
        {
            TodoList? updated = await this.repository.UpdateAsync(ModelToEntity(model));
            if (updated is null)
            {
                throw new UnableToUpdateException(nameof(TodoList), model.Id);
            }

            return EntityToModel(updated);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(TodoList), model.Id, ex);
        }
    }

    private static TodoListModel EntityToModel(TodoList entity)
    {
        var tasks = new List<TodoTaskModel>();
        foreach (var task in entity.TodoTasks)
        {
            var tags = new List<TagModel>();
            foreach (var tag in task.TaskTags)
            {
                tags.Add(new TagModel(tag.Tag.Id, tag.Tag.Label, tag.TaskId, tag.Tag.UserId));
            }

            tasks.Add(new TodoTaskModel(task.Id, task.Title, task.Description, task.CreationDate, task.DueDate, task.StatusId, task.OwnerUserId, task.ListId, new UserModel(task.OwnerUserId, task.OwnerUser.FirstName, task.OwnerUser.LastName), new StatusModel(task.StatusId, task.Status.StatusTitle), tags));
        }

        var userModel = (entity.ListOwner is null) ? null : new UserModel(entity.OwnerId, entity.ListOwner.FirstName, entity.ListOwner.LastName);

        return new TodoListModel(entity.Id, entity.OwnerId, entity.Title, entity.Description, userModel, tasks);
    }

    private static TodoList ModelToEntity(TodoListModel model)
    {
        var entity = new TodoList()
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            OwnerId = model.OwnerId,
        };

        return entity;
    }

    public async Task<TodoListModel?> GetByIdAsync(int listId, int userId)
    {
        var entity = await this.repository.GetByIdAsync(listId);

        if (entity == null || entity.TodoListUserRoles.Any(lur => lur.UserId != userId))
        {
            return null;
        }

        return EntityToModel(entity);
    }
}
