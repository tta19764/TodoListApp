using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database.Interfaces;

/// <summary>
/// Generic repository interface for CRUD operations on entities.
/// </summary>
/// <typeparam name="TEntity">The class inherited from <see cref="BaseEntity"/>.</typeparam>
public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>A read-only list of all entities.</returns>
    Task<IReadOnlyList<TEntity>> GetAllAsync();

    /// <summary>
    /// Gets a paginated list of entities.
    /// </summary>
    /// <param name="pageNumber">The page nuber.</param>
    /// <param name="rowCount">The number of entities on the page.</param>
    /// <returns>A read-only list of entities for the specified page.</returns>
    Task<IReadOnlyList<TEntity>> GetAllAsync(int pageNumber, int rowCount);

    /// <summary>
    /// Gets an entity by its ID.
    /// </summary>
    /// <param name="id">The unique identifier the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<TEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">The new entity.</param>
    /// <returns>The added entity.</returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">An entity to delete.</param>
    /// <returns>True if the entity was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(TEntity entity);

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>True if the entity was deleted; otherwise, false.</returns>
    Task<bool> DeleteByIdAsync(int id);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The new entity state.</param>
    /// <returns>The updated entity if the update was successful; otherwise, null.</returns>
    Task<TEntity?> UpdateAsync(TEntity entity);
}
