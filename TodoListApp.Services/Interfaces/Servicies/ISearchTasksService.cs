using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;
public interface ISearchTasksService
{
    Task<IReadOnlyList<TodoTaskModel>> SearchTasksAsync(int userId, string? title = null, DateTime? creationDate = null, DateTime? dueDate = null, int? pageNumber = null, int? rowCount = null);

    Task<int> GetAllSearchCountAsync(int userId, string? title = null, DateTime? creationDate = null, DateTime? dueDate = null);
}
