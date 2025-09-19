using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Interfaces;

namespace TodoListApp.Services.Database.Repositories;
public class StatusRepository : IStatusRepository
{
    public void Add(Status entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Status entity)
    {
        throw new NotImplementedException();
    }

    public void DeleteById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Status> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Status> GetAll(int pageNumber, int rowCount)
    {
        throw new NotImplementedException();
    }

    public Status? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Status entity)
    {
        throw new NotImplementedException();
    }
}
