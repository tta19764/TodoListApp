using TodoListApp.Services.Enums;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;

/// <summary>
/// Service interface for managing assigned tasks.
/// </summary>
public interface IAssignedTasksService
{
    /// <summary>
    /// Gets all tasks assigned to a user with filtering, sorting, and optional pagination.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="filter">The task status filter.</param>
    /// <param name="sortBy">The sorting property.</param>
    /// <param name="sortOrder">The sorting order.</param>
    /// <param name="pageNumber">The paage number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of tasks assigned to the specified user.</returns>
    Task<IReadOnlyList<TodoTaskModel>> GetAllAssignedAsync(int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null);

    /// <summary>
    /// Gets the count of all tasks assigned to a user with optional filtering.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="filter">The task status filtering.</param>
    /// <returns>The count of tasks assigned to the specified user.</returns>
    Task<int> GetAllAssignedCountAsync(int userId, TaskFilter filter = TaskFilter.Active);
}
