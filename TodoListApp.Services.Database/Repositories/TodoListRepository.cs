using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Interfaces;

namespace TodoListApp.Services.Database.Repositories;
public class TodoListRepository : ITodoListRepository
{
    public void Add(TodoList entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(TodoList entity)
    {
        throw new NotImplementedException();
    }

    public void DeleteById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TodoList> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TodoList> GetAll(int pageNumber, int rowCount)
    {
        throw new NotImplementedException();
    }

    public TodoList? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(TodoList entity)
    {
        throw new NotImplementedException();
    }
}
