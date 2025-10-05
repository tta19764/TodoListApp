using TodoListApp.Services.Enums;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;
public interface ITodoTaskService : IUserCrudService<TodoTaskModel>
{
    Task<IReadOnlyList<TodoTaskModel>> GetAllByListIdAsync(int id, int userId, TaskFilter filter = TaskFilter.Active, string? sortBy = "DueDate", string? sortOrder = "asc", int? pageNumber = null, int? rowCount = null);

    Task<int> GetAllByListIdCountAsync(int id, int userId, TaskFilter filter = TaskFilter.Active);

    Task<TodoTaskModel> UpdateTaskStatusAsync(int userId, int taskId, int statusId);
}
