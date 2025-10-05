using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database.Interfaces;

/// <summary>
/// Repository interface for managing TodoList entities.
/// </summary>
public interface ITodoListRepository : IRepository<TodoList>
{
    /// <summary>
    /// Retrieves all TodoLists associated with a specific user.
    /// </summary>
    /// <param name="userId">The user unique identifier.</param>
    /// <returns>A read-only list of TodoLists.</returns>
    Task<IReadOnlyList<TodoList>> GetAllByUserAsync(int userId);

    /// <summary>
    /// Retrieves a paginated list of TodoLists associated with a specific user.
    /// </summary>
    /// <param name="userId">The user unique identifier of the user.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of entities on the page.</param>
    /// <returns>A read-only list of TodoLists.</returns>
    Task<IReadOnlyList<TodoList>> GetAllByUserAsync(int userId, int pageNumber, int rowCount);

    /// <summary>
    /// Retrieves all TodoLists created by a specific author.
    /// </summary>
    /// <param name="authorId">The unique identifier of the list's owner.</param>
    /// <returns>A read-only list of TodoLists.</returns>
    Task<IReadOnlyList<TodoList>> GetAllByAuthorAsync(int authorId);

    /// <summary>
    /// Retrieves a paginated list of TodoLists created by a specific author.
    /// </summary>
    /// <param name="authorId">The unique identifier of the list's owner.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of entities on the page.</param>
    /// <returns>A read-only list of TodoLists.</returns>
    Task<IReadOnlyList<TodoList>> GetAllByAuthorAsync(int authorId, int pageNumber, int rowCount);

    /// <summary>
    /// Retrieves all TodoLists shared with the user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A read-only list of TodoLists.</returns>
    Task<IReadOnlyList<TodoList>> GetAllSharedAsync(int userId);

    /// <summary>
    /// Retrieves a paginated list of TodoLists shared with the user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of entities on the page.</param>
    /// <returns>A read-only list of TodoLists.</returns>
    Task<IReadOnlyList<TodoList>> GetAllSharedAsync(int userId, int pageNumber, int rowCount);
}
