using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database.Interfaces;
public interface ITodoListRepository : IRepository<TodoList>
{
}
