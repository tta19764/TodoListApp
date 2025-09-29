using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database.Interfaces;

/// <summary>
/// Repository interface for managing task tags.
/// </summary>
public interface ITaskTagRepository : IRepository<TaskTags>
{
}
