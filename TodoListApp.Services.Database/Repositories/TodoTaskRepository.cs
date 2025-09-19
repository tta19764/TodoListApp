using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Interfaces;

namespace TodoListApp.Services.Database.Repositories;
public class TodoTaskRepository : ITodoTaskRepository
{
    public void Add(TodoTask entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(TodoTask entity)
    {
        throw new NotImplementedException();
    }

    public void DeleteById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TodoTask> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TodoTask> GetAll(int pageNumber, int rowCount)
    {
        throw new NotImplementedException();
    }

    public TodoTask? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(TodoTask entity)
    {
        throw new NotImplementedException();
    }
}
