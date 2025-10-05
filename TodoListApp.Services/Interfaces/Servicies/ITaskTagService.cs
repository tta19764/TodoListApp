using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;
public interface ITaskTagService
{
    Task<IReadOnlyList<TagModel>> GetAllUserTaskTagsAsync(int userId, int? pageNumber = null, int? rowCount = null);

    Task<int> GetAllUserTaskTagsCount(int userId);

    Task<int> GetTagTasksCount(int userId, int tagId);

    Task<IReadOnlyList<TodoTaskModel>> GetAllUserTagTasksAsync(int userId, int tagId, int? pageNumber = null, int? rowCount = null);

    Task<TodoTaskModel> AddTaskTag(int userId, int taskId, int tagId);

    Task<TodoTaskModel> RemoveTaskTag(int userId, int taskId, int tagId);
}
