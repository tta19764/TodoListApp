using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Interfaces;

namespace TodoListApp.Services.Database.Repositories;

/// <summary>
/// Repository for managing <see cref="TodoListRole"/> entities.
/// </summary>
public class TodoListRoleRepository : AbstractRepository, ITodoListRoleRepository
{
    private readonly DbSet<TodoListRole> dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListRoleRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    public TodoListRoleRepository(TodoListDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.dbSet = context.Set<TodoListRole>();
    }

    /// <summary>
    /// Asynchronously adds a new <see cref="TodoListRole"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="TodoListRole"/> entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the added <see cref="TodoListRole"/> entity.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public async Task<TodoListRole> AddAsync(TodoListRole entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _ = await this.dbSet.AddAsync(entity);
        _ = await this.Context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Asynchronously deletes <see cref="TodoListRole"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="TodoListRole"/> entity to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <c>bool</c> result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public async Task<bool> DeleteAsync(TodoListRole entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var existing = await this.dbSet.FindAsync(entity.Id);
        if (existing is null)
        {
            return false;
        }

        _ = this.dbSet.Remove(existing);
        _ = await this.Context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Asynchronously deletes <see cref="TodoListRole"/> entity to the repository and saves the changes to the database.
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
    /// Asynchronously gets all <see cref="TodoListRole"/> entities.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoListRole}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoListRole>> GetAllAsync()
    {
        return await this.dbSet
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets set number of <see cref="TodoListRole"/> entities.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoListRole}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoListRole>> GetAllAsync(int pageNumber, int rowCount)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentException("Page number must be greater than 0.");
        }

        if (rowCount < 1)
        {
            throw new ArgumentException("Row count must be greater than 0.");
        }

        return await this.dbSet
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a <see cref="TodoListRole"/> entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>
    /// The <see cref="TodoListRole"/> entity with the specified identifier,
    /// or <c>null</c> if no such entity exists.
    /// </returns>
    public async Task<TodoListRole?> GetByIdAsync(int id)
    {
        return await this.dbSet
            .FindAsync(id);
    }

    /// <summary>
    /// Updates an existing <see cref="TodoListRole"/> entity in the repository.
    /// </summary>
    /// <param name="entity">The <see cref="TodoListRole"/> entity to update.</param>
    /// <returns>Updated <see cref="TodoListRole"/> entity.</returns>
    public async Task<TodoListRole?> UpdateAsync(TodoListRole entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

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
