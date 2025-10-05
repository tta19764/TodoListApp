using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Interfaces;

namespace TodoListApp.Services.Database.Repositories;

/// <summary>
/// Repository for managing <see cref="TaskTags"/> entities.
/// </summary>
public class TaskTagRepository : AbstractRepository, ITaskTagRepository
{
    private readonly DbSet<TaskTags> dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskTagRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    public TaskTagRepository(TodoListDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.dbSet = context.Set<TaskTags>();
    }

    /// <summary>
    /// Asynchronously adds a new <see cref="TaskTags"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="TaskTags"/> entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the added <see cref="TaskTags"/> entity.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public async Task<TaskTags> AddAsync(TaskTags entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _ = await this.dbSet.AddAsync(entity);
        _ = await this.Context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Asynchronously deletes <see cref="TaskTags"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="TaskTags"/> entity to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <c>bool</c> result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public async Task<bool> DeleteAsync(TaskTags entity)
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
    /// Asynchronously deletes <see cref="TaskTags"/> entity to the repository and saves the changes to the database.
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
    /// Asynchronously gets all <see cref="TaskTags"/> entities.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TaskTags}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TaskTags>> GetAllAsync()
    {
        return await this.dbSet
            .Include(tt => tt.Task)
            .Include(tt => tt.Tag)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets set number of <see cref="TaskTags"/> entities.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TaskTags}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TaskTags>> GetAllAsync(int pageNumber, int rowCount)
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
            .Include(tt => tt.Task)
            .Include(tt => tt.Tag)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a <see cref="TaskTags"/> entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>
    /// The <see cref="TaskTags"/> entity with the specified identifier,
    /// or <c>null</c> if no such entity exists.
    /// </returns>
    public async Task<TaskTags?> GetByIdAsync(int id)
    {
        return await this.dbSet
            .Include(tt => tt.Task)
            .Include(tt => tt.Tag)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Updates an existing <see cref="TaskTags"/> entity in the repository.
    /// </summary>
    /// <param name="entity">The <see cref="TaskTags"/> entity to update.</param>
    /// <returns>Updated <see cref="TaskTags"/> entity.</returns>
    public async Task<TaskTags?> UpdateAsync(TaskTags entity)
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
