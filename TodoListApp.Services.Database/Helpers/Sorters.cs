using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database.Helpers;
public static class Sorters
{
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
