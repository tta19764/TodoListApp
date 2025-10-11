using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Interfaces;

namespace TodoListApp.Services.Database.Repositories;

/// <summary>
/// Repository for managing <see cref="Status"/> entities.
/// </summary>
public class StatusRepository : AbstractRepository, IStatusRepository
{
    private readonly DbSet<Status> dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    public StatusRepository(TodoListDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.dbSet = context.Set<Status>();
    }

    /// <summary>
    /// Asynchronously adds a new <see cref="Status"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="Status"/> entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the added <see cref="Status"/> entity.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public Task<Status> AddAsync(Status entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return this.AddInternalAsync(entity);
    }

    /// <summary>
    /// Asynchronously deletes <see cref="Status"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="Status"/> entity to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <c>bool</c> result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public Task<bool> DeleteAsync(Status entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return this.DeleteInternalAsync(entity);
    }

    /// <summary>
    /// Asynchronously deletes <see cref="Status"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="id">The id of the entity to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <c>bool</c> result.
    /// </returns>
    public async Task<bool> DeleteByIdAsync(int id)
    {
        var existing = await this.dbSet.FindAsync(id);
        if (existing is null)
        {
            return false;
        }

        _ = this.dbSet.Remove(existing);
        _ = await this.Context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Asynchronously gets all <see cref="Status"/> entities.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{Status}"/>.
    /// </returns>
    public async Task<IReadOnlyList<Status>> GetAllAsync()
    {
        return await this.dbSet
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets set number of <see cref="Status"/> entities.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{Status}"/>.
    /// </returns>
    public Task<IReadOnlyList<Status>> GetAllAsync(int pageNumber, int rowCount)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentException("Page number must be greater than 0.");
        }

        if (rowCount < 1)
        {
            throw new ArgumentException("Row count must be greater than 0.");
        }

        return this.GetAllInternalAsync(pageNumber, rowCount);
    }

    /// <summary>
    /// Asynchronously retrieves a <see cref="Status"/> entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>
    /// The <see cref="Status"/> entity with the specified identifier,
    /// or <c>null</c> if no such entity exists.
    /// </returns>
    public async Task<Status?> GetByIdAsync(int id)
    {
        return await this.dbSet
            .FindAsync(id);
    }

    /// <summary>
    /// Updates an existing <see cref="Status"/> entity in the repository.
    /// </summary>
    /// <param name="entity">The <see cref="Status"/> entity to update.</param>
    /// <returns>Updated <see cref="Status"/> entity.</returns>
    public Task<Status?> UpdateAsync(Status entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return this.UpdateInternalAsync(entity);
    }

    private async Task<Status> AddInternalAsync(Status entity)
    {
        _ = await this.dbSet.AddAsync(entity);
        _ = await this.Context.SaveChangesAsync();

        return entity;
    }

    private async Task<bool> DeleteInternalAsync(Status entity)
    {
        var existing = await this.dbSet.FindAsync(entity.Id);
        if (existing is null)
        {
            return false;
        }

        _ = this.dbSet.Remove(existing);
        _ = await this.Context.SaveChangesAsync();

        return true;
    }

    private async Task<IReadOnlyList<Status>> GetAllInternalAsync(int pageNumber, int rowCount)
    {
        return await this.dbSet
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToListAsync();
    }

    private async Task<Status?> UpdateInternalAsync(Status entity)
    {
        var existing = await this.dbSet.FindAsync(entity.Id);
        if (existing is null)
        {
            return null;
        }

        this.Context.Entry(existing).CurrentValues.SetValues(entity);
        _ = await this.Context.SaveChangesAsync();

        return existing;
    }
}
