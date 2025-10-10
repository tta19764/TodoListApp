using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;

/// <summary>
/// Service interface for managing to-do lists.
/// </summary>
public interface ITodoListService : IUserCrudService<TodoListModel>
{
    /// <summary>
    /// Gets all to-do lists by user ID with optional pagination.
    /// </summary>
    /// <param name="authorId">The list author ID.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of to-do lists for the specified user.</returns>
    Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId, int? pageNumber = null, int? rowCount = null);

    /// <summary>
    /// Gets all shared to-do lists by user ID with optional pagination.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of shared to-do lists for the specified user.</returns>
    Task<IReadOnlyList<TodoListModel>> GetAllSharedAsync(int userId, int? pageNumber = null, int? rowCount = null);

    /// <summary>
    /// Gets the count of all to-do lists by user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The count of to-do lists for the specified user.</returns>
    Task<int> AllByUserCount(int userId);

    /// <summary>
    /// Gets the count of all to-do lists by author ID.
    /// </summary>
    /// <param name="authorId">The list author ID.</param>
    /// <returns>The count of to-do lists for the specified author.</returns>
    Task<int> AllByAuthorCount(int authorId);

    /// <summary>
    /// Gets the count of all shared to-do lists by user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The count of shared to-do lists for the specified user.</returns>
    Task<int> AllSharedCount(int userId);
}
