using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;

/// <summary>
/// Service interface for searching tasks.
/// </summary>
public interface ISearchTasksService
{
    /// <summary>
    /// Searches for tasks based on various criteria with optional pagination.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="title">The search task title.</param>
    /// <param name="creationDate">The search task creation date.</param>
    /// <param name="dueDate">The search task due date.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of tasks matching the search criteria for the specified user.</returns>
    Task<IReadOnlyList<TodoTaskModel>> SearchTasksAsync(int userId, string? title = null, DateTime? creationDate = null, DateTime? dueDate = null, int? pageNumber = null, int? rowCount = null);

    /// <summary>
    /// Gets the count of all tasks matching the search criteria.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="title">The search task title.</param>
    /// <param name="creationDate">The search task creation date.</param>
    /// <param name="dueDate">The search task due date.</param>
    /// <returns>The count of tasks matching the search criteria for the specified user.</returns>
    Task<int> GetAllSearchCountAsync(int userId, string? title = null, DateTime? creationDate = null, DateTime? dueDate = null);
}
