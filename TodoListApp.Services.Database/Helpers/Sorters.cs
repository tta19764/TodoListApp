using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database.Helpers;

/// <summary>
/// Provides sorting functionality for TodoTask entities.
/// </summary>
public static class Sorters
{
    /// <summary>
    /// Applies sorting to a collection of <see cref="TodoTask"/> entities based on specified criteria.
    /// </summary>
    /// <param name="query">The query with task entities.</param>
    /// <param name="sortBy">The task property to sort by.</param>
    /// <param name="sortOrder">The sorting direction.</param>
    /// <returns>The sorted collection of task entities.</returns>
    public static IEnumerable<TodoTask> ApplyTasksSorting(IEnumerable<TodoTask> query, string sortBy, string sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy))
        {
            sortBy = "Id";
        }

        var isAsc = string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase);

        return sortBy.ToUpperInvariant() switch
        {
            "TITLE" => isAsc
                ? query.OrderBy(t => t.Title)
                : query.OrderByDescending(t => t.Title),

            "DUEDATE" => isAsc
                ? query.OrderBy(t => t.DueDate)
                : query.OrderByDescending(t => t.DueDate),

            _ => isAsc
                ? query.OrderBy(t => t.Id)
                : query.OrderByDescending(t => t.Id)
        };
    }
}
