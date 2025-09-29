using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database.Interfaces;

/// <summary>
/// Repository interface for managing TodoTask entities.
/// </summary>
public interface ITodoTaskRepository : IRepository<TodoTask>
{
    /// <summary>
    /// Gets all tasks associated with a specific TodoList by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the list.</param>
    /// <returns>A read-only list of TodoTask entities.</returns>
    Task<IReadOnlyList<TodoTask>> GetAllByListIdAsync(int id);

    /// <summary>
    /// Gets a paginated list of tasks associated with a specific TodoList by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the list.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>A read-only list of TodoTask entities.</returns>
    Task<IReadOnlyList<TodoTask>> GetAllByListIdAsync(int id, int pageNumber, int rowCount);

    /// <summary>
    /// Gets all tasks associated with a specific user by their user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A read-only list of TodoTask entities.</returns>
    Task<IReadOnlyList<TodoTask>> GetAllUserTasksAsync(int userId);

    /// <summary>
    /// Gets a paginated list of tasks associated with a specific user by their user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>A read-only list of TodoTask entities.</returns>
    Task<IReadOnlyList<TodoTask>> GetAllUserTasksAsync(int userId, int pageNumber, int rowCount);
}
