using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;

/// <summary>
/// Service interface for managing task tags and their associations with tasks.
/// </summary>
public interface ITaskTagService
{
    /// <summary>
    /// Gets all tags associated with a user's tasks with optional pagination.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of tags associated with the user's tasks.</returns>
    Task<IReadOnlyList<TagModel>> GetAllUserTaskTagsAsync(int userId, int? pageNumber = null, int? rowCount = null);

    /// <summary>
    /// Gets the count of all tags associated with a user's tasks.
    /// </summary>
    /// <param name="userId">Teh user ID.</param>
    /// <returns>The count of tags associated with the user's tasks.</returns>
    Task<int> GetAllUserTaskTagsCount(int userId);

    /// <summary>
    /// Gets the count of tasks associated with a specific tag for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="tagId">The tag ID.</param>
    /// <returns>The count of tasks associated with the specified tag for the user.</returns>
    Task<int> GetTagTasksCount(int userId, int tagId);

    /// <summary>
    /// Gets all tasks associated with a specific tag for a user with optional pagination.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="tagId">The tag ID.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of tasks associated with the specified tag for the user.</returns>
    Task<IReadOnlyList<TodoTaskModel>> GetAllUserTagTasksAsync(int userId, int tagId, int? pageNumber = null, int? rowCount = null);

    /// <summary>
    /// Associates a tag with a task for a user.
    /// </summary>
    /// <param name="userId">Teh user ID.</param>
    /// <param name="taskId">The task ID.</param>
    /// <param name="tagId">The tag ID.</param>
    /// <returns>The updated task with the associated tag.</returns>
    Task<TodoTaskModel> AddTaskTag(int userId, int taskId, int tagId);

    /// <summary>
    /// Removes the association of a tag from a task for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="taskId">The task ID.</param>
    /// <param name="tagId">The tag ID.</param>
    /// <returns>The updated task without the removed tag.</returns>
    Task<TodoTaskModel> RemoveTaskTag(int userId, int taskId, int tagId);
}
