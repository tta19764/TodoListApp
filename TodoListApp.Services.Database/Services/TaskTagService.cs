using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Helpers;
using TodoListApp.Services.Database.Repositories;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Services;
public class TaskTagService : ITaskTagService
{
    private readonly TodoTaskRepository repository;

    public TaskTagService(TodoListDbContext context)
    {
        this.repository = new TodoTaskRepository(context);
    }

    public async Task<int> GetAllUserTaskTagsCount(int userId)
    {
        var tasks = await this.repository.GetAllUserTasksAsync(userId);

        return tasks
        .SelectMany(t => t.TaskTags)
        .Select(tt => tt.Tag)
        .Distinct()
        .Count();
    }

    public async Task<IReadOnlyList<TagModel>> GetAllUserTaskTagsAsync(int userId, int? pageNumber = null, int? rowCount = null)
    {
        var tasks = await this.repository.GetAllUserTasksAsync(userId);
        var tagsQuery = tasks
            .SelectMany(t => t.TaskTags)
            .Select(tt => tt.Tag)
            .Distinct()
            .AsEnumerable();

        if (pageNumber != null && rowCount != null)
        {
            int page = pageNumber > 0 ? (int)pageNumber : 1;
            int row = rowCount > 0 ? (int)rowCount : 1;
            tagsQuery = tagsQuery
                .Skip((page - 1) * row)
                .Take(row)
                .AsEnumerable();
        }

        return tagsQuery
            .Select(t => new TagModel(t.Id, t.Label))
            .ToList();
    }

    public async Task<int> GetTagTasksCount(int userId, int tagId)
    {
        var tasks = await this.repository.GetAllUserTasksAsync(userId);
        return tasks
            .Count(t => t.TaskTags.Any(tt => tt.TagId == tagId));
    }

    public async Task<IReadOnlyList<TodoTaskModel>> GetAllUserTagTasksAsync(int userId, int tagId, int? pageNumber = null, int? rowCount = null)
    {
        var tasks = await this.repository.GetAllUserTasksAsync(userId);
        var taggedTasksQuery = tasks
            .Where(t => t.TaskTags.Any(tt => tt.TagId == tagId))
            .AsEnumerable();

        if (pageNumber != null && rowCount != null)
        {
            int page = pageNumber > 0 ? (int)pageNumber : 1;
            int row = rowCount > 0 ? (int)rowCount : 1;
            taggedTasksQuery = taggedTasksQuery
                .Skip((page - 1) * row)
                .Take(row)
                .AsEnumerable();
        }

        return taggedTasksQuery
            .Select(t => EntityToModelConverter.ToTodoTaskModel(t))
            .ToList();
    }

    public async Task<TodoTaskModel> AddTaskTag(int userId, int taskId, int tagId)
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
            existing.TaskTags.Add(new TaskTags() { TaskId = taskId, TagId = tagId });
            var updated = await this.repository.UpdateAsync(existing);
            return updated is null ?
                throw new UnableToUpdateException(nameof(existing), taskId) :
                EntityToModelConverter.ToTodoTaskModel(updated);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(existing), taskId, ex);
        }
    }

    public async Task<TodoTaskModel> RemoveTaskTag(int userId, int taskId, int tagId)
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
            var taskTag = existing.TaskTags.FirstOrDefault(tt => tt.TagId == tagId && tt.TaskId == taskId);
            if (taskTag != null)
            {
                _ = existing.TaskTags.Remove(taskTag);
            }

            var updated = await this.repository.UpdateAsync(existing);
            return updated is null ?
                throw new UnableToUpdateException(nameof(existing), taskId) :
                EntityToModelConverter.ToTodoTaskModel(updated);
        }
        catch (DbUpdateException ex)
        {
            throw new UnableToUpdateException(nameof(existing), taskId, ex);
        }
    }
}
