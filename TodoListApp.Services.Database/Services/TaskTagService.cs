using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Helpers;
using TodoListApp.Services.Database.Repositories;
using TodoListApp.Services.Database.ServiceExeptions;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Services;

/// <summary>
/// Provides functionality for managing tag assignments to tasks,
/// including retrieval, association, and removal of tags for user tasks.
/// </summary>
public class TaskTagService : ITaskTagService
{
    private readonly TodoTaskRepository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskTagService"/> class
    /// using the specified database context.
    /// </summary>
    /// <param name="context">The <see cref="TodoListDbContext"/> used for database operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <c>null</c>.</exception>
    public TaskTagService(TodoListDbContext context)
    {
        this.repository = new TodoTaskRepository(context);
    }

    /// <summary>
    /// Retrieves the total number of distinct tags associated with all tasks owned or shared with a user.
    /// </summary>
    /// <param name="userId">The ID of the user whose task tags should be counted.</param>
    /// <returns>The total number of unique tags associated with the user's tasks.</returns>
    public async Task<int> GetAllUserTaskTagsCount(int userId)
    {
        var tasks = await this.repository.GetAllUserTasksAsync(userId);

        return tasks
        .SelectMany(t => t.TaskTags)
        .Select(tt => tt.Tag)
        .Distinct()
        .Count();
    }

    /// <summary>
    /// Retrieves all unique tags associated with tasks owned or shared with a user.
    /// </summary>
    /// <param name="userId">The ID of the user whose task tags should be retrieved.</param>
    /// <param name="pageNumber">Optional page number for paginated retrieval (1-based).</param>
    /// <param name="rowCount">Optional number of records per page.</param>
    /// <returns>A read-only list of <see cref="TagModel"/> representing the tags linked to the user's tasks.</returns>
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
                .OrderBy(x => x.Id)
                .Skip((page - 1) * row)
                .Take(row)
                .AsEnumerable();
        }

        return tagsQuery
            .Select(t => new TagModel(t.Id, t.Label))
            .ToList();
    }

    /// <summary>
    /// Retrieves the number of tasks belonging to a user that are tagged with a specific tag.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="tagId">The ID of the tag to count tasks for.</param>
    /// <returns>The number of tasks tagged with the specified tag.</returns>
    public async Task<int> GetTagTasksCount(int userId, int tagId)
    {
        var tasks = await this.repository.GetAllUserTasksAsync(userId);
        return tasks
            .Count(t => t.TaskTags.Any(tt => tt.TagId == tagId));
    }

    /// <summary>
    /// Retrieves all tasks belonging to a user that are tagged with a specific tag.
    /// </summary>
    /// <param name="userId">The ID of the user whose tasks are retrieved.</param>
    /// <param name="tagId">The ID of the tag to filter tasks by.</param>
    /// <param name="pageNumber">Optional page number for paginated retrieval (1-based).</param>
    /// <param name="rowCount">Optional number of records per page.</param>
    /// <returns>A read-only list of <see cref="TodoTaskModel"/> tagged with the specified tag.</returns>
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

    /// <summary>
    /// Adds a tag to a specified task if the user has permission to modify the task.
    /// </summary>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <param name="taskId">The ID of the task to which the tag should be added.</param>
    /// <param name="tagId">The ID of the tag to associate with the task.</param>
    /// <returns>The updated <see cref="TodoTaskModel"/> with the newly added tag.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the specified task does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to modify the task.</exception>
    /// <exception cref="UnableToUpdateException">Thrown when the database operation fails to update the task.</exception>
    public async Task<TodoTaskModel> AddTaskTag(int userId, int taskId, int tagId)
    {
        TodoTask? existing = await this.repository.GetByIdAsync(taskId);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), taskId);
        }
        else if (existing.TodoList.OwnerId != userId &&
            !existing.TodoList.TodoListUserRoles.Any(lur => lur.UserId == userId && lur.ListRole.RoleName == "Editor"))
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

    /// <summary>
    /// Removes a tag from a specified task if the user has permission to modify the task.
    /// </summary>
    /// <param name="userId">The ID of the user performing the operation.</param>
    /// <param name="taskId">The ID of the task from which the tag should be removed.</param>
    /// <param name="tagId">The ID of the tag to remove from the task.</param>
    /// <returns>The updated <see cref="TodoTaskModel"/> without the removed tag.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the specified task does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to modify the task.</exception>
    /// <exception cref="UnableToUpdateException">Thrown when the database operation fails to update the task.</exception>
    public async Task<TodoTaskModel> RemoveTaskTag(int userId, int taskId, int tagId)
    {
        TodoTask? existing = await this.repository.GetByIdAsync(taskId);
        if (existing is null)
        {
            throw new EntityNotFoundException(nameof(existing), taskId);
        }
        else if (existing.TodoList.OwnerId != userId &&
            !existing.TodoList.TodoListUserRoles.Any(lur => lur.UserId == userId && lur.ListRole.RoleName == "Editor"))
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
