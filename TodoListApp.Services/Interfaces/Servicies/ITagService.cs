using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;
public interface ITagService : ICrudService<TagModel>
{
    Task<IReadOnlyList<TagModel>> GetAvilableTaskTagsAsync(int taskId, int? pageNumber = null, int? rowCount = null);

    Task<int> GetAvilableTaskTagsCountAsync(int taskId);
}
