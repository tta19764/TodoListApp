using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;
public interface ITodoListService : IUserCrudService<TodoListModel>
{
    Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId, int? pageNumber = null, int? rowCount = null);

    Task<IReadOnlyList<TodoListModel>> GetAllSharedAsync(int userId, int? pageNumber = null, int? rowCount = null);

    Task<int> AllByUserCount(int userId);

    Task<int> AllByAuthorCount(int authorId);

    Task<int> AllSharedCount(int userId);
}
