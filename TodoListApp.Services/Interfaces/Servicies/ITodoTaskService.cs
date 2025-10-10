using TodoListApp.Services.Enums;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;

/// <summary>
/// Service interface for managing to-do tasks.
/// </summary>
public interface ITodoTaskService : IUserCrudService<TodoTaskModel>
{
    /// <summary>
    /// Gets all tasks by list ID with optional filtering, sorting, and pagination.
    /// </summary>
    /// <param name="id">The list ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="filter">The task status filter.</param>
    /// <param name="sortBy">The property to sort by.</param>
    /// <param name="sortOrder">The sorting order.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of tasks for the specified list and user.</returns>
    Task<IReadOnlyList<TodoTaskModel>> GetAllByListIdAsync(int id, int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null);

    /// <summary>
    /// Gets the count of all tasks by list ID with optional filtering.
    /// </summary>
    /// <param name="id">The list ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="filter">The task status filter.</param>
    /// <returns>The count of tasks for the specified list and user.</returns>
    Task<int> GetAllByListIdCountAsync(int id, int userId, TaskFilter filter = TaskFilter.Active);

    /// <summary>
    /// Updates the status of a task.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="taskId">The task ID.</param>
    /// <param name="statusId">The task status ID.</param>
    /// <returns>The updated task.</returns>
    Task<TodoTaskModel> UpdateTaskStatusAsync(int userId, int taskId, int statusId);

    /// <summary>
    /// Gets the count of comments for a specific task.
    /// </summary>
    /// <param name="taskId">The tas kID.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>The count of comments for the specified task and user.</returns>
    Task<int> GetTaskCommentsCountAsync(int taskId, int userId);

    /// <summary>
    /// Gets comments for a specific task with optional pagination.
    /// </summary>
    /// <param name="taskId">The task ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="pageNumber">The page numebr.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of comments for the specified task and user.</returns>
    Task<IReadOnlyList<CommentModel>> GetTaskCommentsAsync(int taskId, int userId, int? pageNumber = null, int? rowCount = null);

    /// <summary>
    /// Adds a comment to a task.
    /// </summary>
    /// <param name="commentModel">The new comment model.</param>
    /// <returns>The added comment.</returns>
    Task<CommentModel> AddTaskCommentAsync(CommentModel commentModel);

    /// <summary>
    /// Updates a task comment.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="commentModel">The comment model to update.</param>
    /// <returns>The updated comment.</returns>
    Task<CommentModel> UpdateTaskCommentAsync(int userId, CommentModel commentModel);

    /// <summary>
    /// Gets a comment by its ID for a specific task and user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="taskId">The tas kID.</param>
    /// <param name="commentId">The comment ID.</param>
    /// <returns>The comment with the specified ID.</returns>
    Task<CommentModel> GetCommentByIdAsync(int userId, int taskId, int commentId);

    /// <summary>
    /// Removes a comment from a task.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="taskId">The tas kID.</param>
    /// <param name="commentId">The comment ID.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveTaskCommentAsync(int userId, int taskId, int commentId);
}
