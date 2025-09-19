using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Interfaces;

namespace TodoListApp.Services.Database.Repositories;
public class CommentRepository : ICommentRepository
{
    public void Add(Comment entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Comment entity)
    {
        throw new NotImplementedException();
    }

    public void DeleteById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Comment> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Comment> GetAll(int pageNumber, int rowCount)
    {
        throw new NotImplementedException();
    }

    public Comment? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Comment entity)
    {
        throw new NotImplementedException();
    }
}
