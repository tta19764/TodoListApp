using TodoListApp.Services.Enums;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;
public interface IAssignedTasksService
{
    Task<IReadOnlyList<TodoTaskModel>> GetAllAssignedAsync(int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null);

    Task<int> GetAllAssignedCountAsync(int userId, TaskFilter filter = TaskFilter.Active);
}
