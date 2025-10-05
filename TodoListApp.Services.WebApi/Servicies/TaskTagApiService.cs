using Microsoft.Extensions.Logging;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.WebApi.Servicies;
public class TaskTagApiService : ITaskTagService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<TaskTagApiService> logger;

    public TaskTagApiService(HttpClient httpClient, ILogger<TaskTagApiService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<TodoTaskModel> AddTaskTag(int userId, int taskId, int tagId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TodoTaskModel>> GetAllUserTagTasksAsync(int userId, int tagId, int? pageNumber = null, int? rowCount = null)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TagModel>> GetAllUserTaskTagsAsync(int userId, int? pageNumber = null, int? rowCount = null)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetAllUserTaskTagsCount(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTagTasksCount(int userId, int tagId)
    {
        throw new NotImplementedException();
    }

    public Task<TodoTaskModel> RemoveTaskTag(int userId, int taskId, int tagId)
    {
        throw new NotImplementedException();
    }
}
