using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database.Interfaces;

/// <summary>
/// Repository interface for managing Comment entities.
/// </summary>
public interface ICommentRepository : IRepository<Comment>
{
}
