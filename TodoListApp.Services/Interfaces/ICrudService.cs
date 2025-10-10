using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces;

/// <summary>
/// Generic CRUD service interface for managing entites.
/// </summary>
/// <typeparam name="TModel">The model that inherits from abstract model.</typeparam>
public interface ICrudService<TModel>
    where TModel : AbstractModel
{
    /// <summary>
    /// Gets all models.
    /// </summary>
    /// <returns>A read-only list of all models.</returns>
    Task<IReadOnlyList<TModel>> GetAllAsync();

    /// <summary>
    /// Gets a paginated list of models.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="rowCount">The number of models on the page.</param>
    /// <returns>A read-only list of models for the specified page.</returns>
    Task<IReadOnlyList<TModel>> GetAllAsync(int pageNumber, int rowCount);

    /// <summary>
    /// Gets an entity by its ID.
    /// </summary>
    /// <param name="id">The model ID.</param>
    /// <returns>The model with the specified ID.</returns>
    Task<TModel> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new model.
    /// </summary>
    /// <param name="model">The new model.</param>
    /// <returns>The added model.</returns>
    Task<TModel> AddAsync(TModel model);

    /// <summary>
    /// Deletes a model by its ID.
    /// </summary>
    /// <param name="id">The model ID.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(int id);

    /// <summary>
    /// Updates an existing model.
    /// </summary>
    /// <param name="model">The model to update.</param>
    /// <returns>The updated model.</returns>
    Task<TModel> UpdateAsync(TModel model);
}
