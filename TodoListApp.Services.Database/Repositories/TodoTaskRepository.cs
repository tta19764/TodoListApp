using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Interfaces;

namespace TodoListApp.Services.Database.Repositories;

/// <summary>
/// Repository for managing <see cref="TodoTask"/> entities.
/// </summary>
public class TodoTaskRepository : AbstractRepository, ITodoTaskRepository
{
    private readonly DbSet<TodoTask> dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTaskRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    public TodoTaskRepository(TodoListDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.dbSet = context.Set<TodoTask>();
    }

    /// <summary>
    /// Asynchronously adds a new <see cref="TodoTask"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="TodoTask"/> entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the added <see cref="TodoTask"/> entity.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public async Task<TodoTask> AddAsync(TodoTask entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _ = await this.dbSet.AddAsync(entity);
        _ = await this.Context.SaveChangesAsync();

        return await this.dbSet
        .Include(t => t.TodoList)
        .Include(t => t.OwnerUser)
        .Include(t => t.Status)
        .Include(t => t.TaskTags)
            .ThenInclude(tt => tt.Tag)
        .Include(t => t.Comments)
        .FirstAsync(e => e.Id == entity.Id);
    }

    /// <summary>
    /// Asynchronously deletes <see cref="TodoTask"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="TodoTask"/> entity to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <c>bool</c> result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public async Task<bool> DeleteAsync(TodoTask entity)
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
    /// Asynchronously deletes <see cref="TodoTask"/> entity to the repository and saves the changes to the database.
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
    /// Asynchronously gets all <see cref="TodoTask"/> entities.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoTask}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoTask>> GetAllAsync()
    {
        return await this.dbSet
            .Include(t => t.TodoList)
            .Include(t => t.OwnerUser)
            .Include(t => t.Status)
            .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
            .Include(t => t.Comments)
            .AsSplitQuery()
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets set number of <see cref="TodoTask"/> entities.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoTask}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoTask>> GetAllAsync(int pageNumber, int rowCount)
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
            .Include(t => t.TodoList)
            .Include(t => t.OwnerUser)
            .Include(t => t.Status)
            .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
            .Include(t => t.Comments)
            .AsSplitQuery()
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets all <see cref="TodoTask"/> entities by list inique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the list.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoTask}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoTask>> GetAllByListIdAsync(int id)
    {
        return await this.dbSet
            .Where(t => t.ListId == id)
            .Include(t => t.TodoList)
            .Include(t => t.OwnerUser)
            .Include(t => t.Status)
            .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
            .Include(t => t.Comments)
            .AsSplitQuery()
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets all <see cref="TodoTask"/> entities by list inique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the list.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoTask}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoTask>> GetAllByListIdAsync(int id, int pageNumber, int rowCount)
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
            .Where(t => t.ListId == id)
            .Include(t => t.TodoList)
            .Include(t => t.OwnerUser)
            .Include(t => t.Status)
            .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
            .Include(t => t.Comments)
            .AsSplitQuery()
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets all <see cref="TodoTask"/> entities by user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoTask}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoTask>> GetAllUserTasksAsync(int userId)
    {
        return await this.dbSet
            .Where(t => t.OwnerUserId == userId)
            .Include(t => t.TodoList)
            .Include(t => t.OwnerUser)
            .Include(t => t.Status)
            .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
            .Include(t => t.Comments)
            .AsSplitQuery()
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets all <see cref="TodoTask"/> entities by user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoTask}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoTask>> GetAllUserTasksAsync(int userId, int pageNumber, int rowCount)
    {
        return await this.dbSet
            .Where(t => t.OwnerUserId == userId)
            .Include(t => t.TodoList)
            .Include(t => t.OwnerUser)
            .Include(t => t.Status)
            .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
            .Include(t => t.Comments)
            .AsSplitQuery()
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a <see cref="TodoTask"/> entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>
    /// The <see cref="TodoTask"/> entity with the specified identifier,
    /// or <c>null</c> if no such entity exists.
    /// </returns>
    public async Task<TodoTask?> GetByIdAsync(int id)
    {
        return await this.dbSet
            .Include(t => t.TodoList)
            .Include(t => t.OwnerUser)
            .Include(t => t.Status)
            .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
            .Include(t => t.Comments)
            .AsSplitQuery()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Updates an existing <see cref="TodoTask"/> entity in the repository.
    /// </summary>
    /// <param name="entity">The <see cref="TodoTask"/> entity to update.</param>
    /// <returns>Updated <see cref="TodoTask"/> entity.</returns>
    public async Task<TodoTask?> UpdateAsync(TodoTask entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var existing = await this.dbSet.FindAsync(entity.Id);
        if (existing is null)
        {
            return null;
        }

        this.Context.Entry(existing).CurrentValues.SetValues(entity);
        _ = await this.Context.SaveChangesAsync();

        return await this.dbSet
            .Include(t => t.TodoList)
            .Include(t => t.OwnerUser)
            .Include(t => t.Status)
            .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == existing.Id);
    }
}
