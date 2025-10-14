using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Interfaces;

namespace TodoListApp.Services.Database.Repositories;

/// <summary>
/// Repository for managing <see cref="Comment"/> entities.
/// </summary>
public class CommentRepository : AbstractRepository, ICommentRepository
{
    private readonly DbSet<Comment> dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    public CommentRepository(TodoListDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.dbSet = context.Set<Comment>();
    }

    /// <summary>
    /// Asynchronously adds a new <see cref="Comment"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="Comment"/> entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the added <see cref="Comment"/> entity.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public Task<Comment> AddAsync(Comment entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return this.AddInternalAsync(entity);
    }

    /// <summary>
    /// Asynchronously deletes <see cref="Comment"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="Comment"/> entity to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <c>bool</c> result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public Task<bool> DeleteAsync(Comment entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return this.DeleteInternalAsync(entity);
    }

    /// <summary>
    /// Asynchronously deletes <see cref="Comment"/> entity to the repository and saves the changes to the database.
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
    /// Asynchronously gets all <see cref="Comment"/> entities.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{Comment}"/>.
    /// </returns>
    public async Task<IReadOnlyList<Comment>> GetAllAsync()
    {
        return await this.dbSet
            .Include(c => c.Author)
            .Include(c => c.Task)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets set number of <see cref="Comment"/> entities.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{Comment}"/>.
    /// </returns>
    public Task<IReadOnlyList<Comment>> GetAllAsync(int pageNumber, int rowCount)
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
    /// Asynchronously retrieves a <see cref="Comment"/> entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>
    /// The <see cref="Comment"/> entity with the specified identifier,
    /// or <c>null</c> if no such entity exists.
    /// </returns>
    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await this.dbSet
            .Include(c => c.Author)
            .Include(c => c.Task)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Updates an existing <see cref="Comment"/> entity in the repository.
    /// </summary>
    /// <param name="entity">The <see cref="Comment"/> entity to update.</param>
    /// <returns>Updated <see cref="Comment"/> entity.</returns>
    public Task<Comment?> UpdateAsync(Comment entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return this.UpdateInternalAsync(entity);
    }

    private async Task<Comment> AddInternalAsync(Comment entity)
    {
        _ = await this.dbSet.AddAsync(entity);
        _ = await this.Context.SaveChangesAsync();

        await this.Context.Entry(entity)
        .Reference(e => e.Author)
        .LoadAsync();

        return entity;
    }

    private async Task<bool> DeleteInternalAsync(Comment entity)
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

    private async Task<IReadOnlyList<Comment>> GetAllInternalAsync(int pageNumber, int rowCount)
    {
        return await this.dbSet
            .Include(c => c.Author)
            .Include(c => c.Task)
            .OrderBy(t => t.Id)
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToListAsync();
    }

    private async Task<Comment?> UpdateInternalAsync(Comment entity)
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
