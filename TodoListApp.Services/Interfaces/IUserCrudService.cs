using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces;

/// <summary>
/// Generic CRUD service interface for managing user-specific entities.
/// </summary>
/// <typeparam name="TModel">The model that inherits from abstract model.</typeparam>
public interface IUserCrudService<TModel>
    where TModel : AbstractModel
{
    /// <summary>
    /// Gets all models for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A read-only list of all models for the specified user.</returns>
    Task<IReadOnlyList<TModel>> GetAllAsync(int userId);

    /// <summary>
    /// Gets a paginated list of models for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of models for the specified user and page.</returns>
    Task<IReadOnlyList<TModel>> GetAllAsync(int userId, int pageNumber, int rowCount);

    /// <summary>
    /// Gets an entity by its ID for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="id">The model ID.</param>
    /// <returns>The model with the specified ID for the specified user.</returns>
    Task<TModel> GetByIdAsync(int userId, int id);

    /// <summary>
    /// Adds a new model for a specific user.
    /// </summary>
    /// <param name="model">The new model.</param>
    /// <returns>The added model.</returns>
    Task<TModel> AddAsync(TModel model);

    /// <summary>
    /// Deletes a model by its ID for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="id">The model ID.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(int userId, int id);

    /// <summary>
    /// Updates an existing model for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="model">The model to update.</param>
    /// <reteurns>The updated model.</returns>
    Task<TModel> UpdateAsync(int userId, TModel model);
}
