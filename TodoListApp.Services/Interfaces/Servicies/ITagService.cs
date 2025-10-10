using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces.Servicies;

/// <summary>
/// Service interface for managing tags.
/// </summary>
public interface ITagService : ICrudService<TagModel>
{
    /// <summary>
    /// Gets all tags available for a specific task with optional pagination.
    /// </summary>
    /// <param name="taskId">The tsk ID.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of tags available for the specified task.</returns>
    Task<IReadOnlyList<TagModel>> GetAvailableTaskTagsAsync(int taskId, int? pageNumber = null, int? rowCount = null);

    /// <summary>
    /// Gets the count of all tags available for a specific task.
    /// </summary>
    /// <param name="taskId">Teh task ID.</param>
    /// <returns>The count of tags available for the specified task.</returns>
    Task<int> GetAvilableTaskTagsCountAsync(int taskId);
}
