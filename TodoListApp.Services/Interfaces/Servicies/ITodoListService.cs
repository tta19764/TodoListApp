using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;
public interface ITodoListService : ICrudService<TodoListModel>
{
    Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId);

    Task<IReadOnlyList<TodoListModel>> GetAllByAuthorAsync(int authorId, int pageNumber, int rowCount);
}
