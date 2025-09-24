using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;
public interface ITodoListService : ICrudService<TodoListModel>
{
    Task<IReadOnlyList<TodoListModel>> GetAllByUserAsync(int userId);

    Task<IReadOnlyList<TodoListModel>> GetAllByUserAsync(int userId, int pageNumber, int rowCount);

    Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId);

    Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId, int pageNumber, int rowCount);

    Task<TodoListModel?> GetByIdAsync(int listId, int userId);
}
