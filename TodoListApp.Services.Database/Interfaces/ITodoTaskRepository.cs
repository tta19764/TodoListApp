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

    /// <summary>
    /// Searches for tasks based on optional criteria: title, creation date, and due date.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="title">The task title.</param>
    /// <param name="creationDate">The task creation date.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <returns>A read-only list of TodoTask entities.</returns>
    Task<IReadOnlyList<TodoTask>> SerchTasksAsync(int userId, string? title, DateTime? creationDate, DateTime? dueDate);

    /// <summary>
    /// Searches for tasks based on optional criteria: title, creation date, and due date with pagination.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="title">The task title.</param>
    /// <param name="creationDate">The task creation date.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>A read-only list of TodoTask entities.</returns>
    Task<IReadOnlyList<TodoTask>> SerchTasksAsync(int userId, string? title, DateTime? creationDate, DateTime? dueDate, int pageNumber, int rowCount);

    /// <summary>
    /// Gets all tasks assigned to a specific user by their user ID.
    /// </summary>
    /// <param name="userId">The user unique identifier.</param>
    /// <returns>A read-only list of TodoTask entities.</returns>
    Task<IReadOnlyList<TodoTask>> GetAllAssignedTasksAsync(int userId);

    /// <summary>
    /// Gets a paginated list of tasks assigned to a specific user by their user ID.
    /// </summary>
    /// <param name="userId">The user unique identifier.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>A read-only list of TodoTask entities.</returns>
    Task<IReadOnlyList<TodoTask>> GetAllAssignedTasksAsync(int userId, int pageNumber, int rowCount);
}
