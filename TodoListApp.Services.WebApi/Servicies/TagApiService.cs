using Microsoft.Extensions.Logging;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.Models;

namespace TodoListApp.Services.WebApi.Servicies;
public class TagApiService : ITagService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<TagApiService> logger;

    public TagApiService(ILogger<TagApiService> logger, HttpClient httpClient)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public Task<TagModel> AddAsync(TagModel model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TagModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TagModel>> GetAllAsync(int pageNumber, int rowCount)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TagModel>> GetAvilableTaskTagsAsync(int taskId, int? pageNumber = null, int? rowCount = null)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetAvilableTaskTagsCountAsync(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task<TagModel> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<TagModel> UpdateAsync(TagModel model)
    {
        throw new NotImplementedException();
    }
}
