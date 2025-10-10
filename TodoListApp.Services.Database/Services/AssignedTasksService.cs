using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Helpers;
using TodoListApp.Services.Database.Repositories;
using TodoListApp.Services.Enums;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Database.Services;
public class AssignedTasksService : IAssignedTasksService
{
    private readonly TodoTaskRepository repository;

    public AssignedTasksService(TodoListDbContext context)
    {
        this.repository = new TodoTaskRepository(context);
    }

    /// <summary>
    /// Gets all to-do tasks created by the specified user with filtering, sorting, and pagination.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="filter">The filtering by task status.</param>
    /// <param name="sortBy">The sorting property.</param>
    /// <param name="sortOrder">The sortyng direction.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of tasks on the page.</param>
    /// <returns>A read-only list of to-do task models.</returns>
    public async Task<IReadOnlyList<TodoTaskModel>> GetAllAssignedAsync(int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null)
    {
        sortOrder = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase) ? sortOrder : "asc";
        sortBy = string.Equals(sortBy, "Title", StringComparison.OrdinalIgnoreCase) ? sortBy : "DueDate";

        var tasks = await this.repository.GetAllAssignedTasksAsync(userId);

        var filteredTasks = filter switch
        {
            TaskFilter.InProgress => tasks.Where(t => t.StatusId == 2),
            TaskFilter.NotStarted => tasks.Where(t => t.StatusId == 1),
            TaskFilter.Completed => tasks.Where(t => t.StatusId == 3),
            TaskFilter.Active => tasks.Where(t => t.StatusId != 3),
            TaskFilter.All => tasks,
            _ => tasks
        };

        var sortedTasks = Sorters.ApplyTasksSorting(filteredTasks.AsQueryable(), sortBy!, sortOrder!);

        if (pageNumber != null && rowCount != null)
        {
            int page = pageNumber > 0 ? (int)pageNumber : 1;
            int row = rowCount > 0 ? (int)rowCount : 1;

            sortedTasks = sortedTasks
                .Skip((page - 1) * row)
                .Take(row)
                .AsQueryable();
        }

        return sortedTasks.Select(t => EntityToModelConverter.ToTodoTaskModel(t)).ToList();
    }

    /// <summary>
    /// Gets the count of to-do tasks created by the specified user with filtering.
    /// </summary>
    /// <param name="userId">The unique user identifier.</param>
    /// <param name="filter">The status filtering.</param>
    /// <returns>The count of to-do tasks.</returns>
    public async Task<int> GetAllAssignedCountAsync(int userId, TaskFilter filter = TaskFilter.Active)
    {
        var tasks = await this.repository.GetAllAssignedTasksAsync(userId);

        int count = filter switch
        {
            TaskFilter.InProgress => tasks.Count(t => t.StatusId == 2),
            TaskFilter.NotStarted => tasks.Count(t => t.StatusId == 1),
            TaskFilter.Completed => tasks.Count(t => t.StatusId == 3),
            TaskFilter.Active => tasks.Count(t => t.StatusId != 3),
            TaskFilter.All => tasks.Count,
            _ => tasks.Count
        };

        return count;
    }
}
