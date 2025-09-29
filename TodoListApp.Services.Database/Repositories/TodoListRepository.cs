using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Interfaces;

namespace TodoListApp.Services.Database.Repositories;

/// <summary>
/// Repository for managing <see cref="TodoList"/> entities.
/// </summary>
public class TodoListRepository : AbstractRepository, ITodoListRepository
{
    private readonly DbSet<TodoList> dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    public TodoListRepository(TodoListDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
        this.dbSet = this.Context.Set<TodoList>();
    }

    /// <summary>
    /// Asynchronously adds a new <see cref="TodoList"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="TodoList"/> entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the added <see cref="TodoList"/> entity.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public async Task<TodoList> AddAsync(TodoList entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _ = await this.dbSet.AddAsync(entity);
        _ = await this.Context.SaveChangesAsync();

        await this.Context.Entry(entity)
        .Reference(e => e.ListOwner)
        .LoadAsync();

        return entity;
    }

    /// <summary>
    /// Asynchronously deletes <see cref="TodoList"/> entity to the repository and saves the changes to the database.
    /// </summary>
    /// <param name="entity">The <see cref="TodoList"/> entity to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <c>bool</c> result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    public async Task<bool> DeleteAsync(TodoList entity)
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
    /// Asynchronously deletes <see cref="TodoList"/> entity to the repository and saves the changes to the database.
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
    /// Asynchronously gets all <see cref="TodoList"/> entities.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoList}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoList>> GetAllAsync()
    {
        return await this.dbSet
        .Include(tl => tl.TodoTasks)
                .ThenInclude(tt => tt.Status)
        .Include(tl => tl.ListOwner)
        .Include(tl => tl.TodoListUserRoles)
            .ThenInclude(tlur => tlur.ListRole)
            .AsSplitQuery()
        .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets set number of <see cref="TodoList"/> entities.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoList}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoList>> GetAllAsync(int pageNumber, int rowCount)
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
            .Include(tl => tl.TodoTasks)
                .ThenInclude(tt => tt.Status)
            .Include(tl => tl.ListOwner)
        .Include(tl => tl.TodoListUserRoles)
            .ThenInclude(tlur => tlur.ListRole)
            .AsSplitQuery()
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets the list of <see cref="TodoList"/> entities by author.
    /// </summary>
    /// <param name="authorId">The unique identifier of the task author.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoList}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoList>> GetAllByAuthorAsync(int authorId)
    {
        return await this.dbSet
            .Where(l => l.OwnerId == authorId)
            .Include(tl => tl.TodoTasks)
                .ThenInclude(tt => tt.Status)
            .Include(tl => tl.ListOwner)
        .Include(tl => tl.TodoListUserRoles)
            .ThenInclude(tlur => tlur.ListRole)
            .AsSplitQuery()
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets the list of <see cref="TodoList"/> entities by author.
    /// </summary>
    /// <param name="authorId">The unique identifier of the task author.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoList}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoList>> GetAllByAuthorAsync(int authorId, int pageNumber, int rowCount)
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
            .Where(l => l.OwnerId == authorId)
            .Include(tl => tl.TodoTasks)
                .ThenInclude(tt => tt.Status)
            .Include(tl => tl.ListOwner)
        .Include(tl => tl.TodoListUserRoles)
            .ThenInclude(tlur => tlur.ListRole)
            .AsSplitQuery()
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets the list of <see cref="TodoList"/> entities that the user has access to.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoList}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoList>> GetAllByUserAsync(int userId)
    {
        return await this.dbSet
            .Where(l => l.OwnerId == userId || l.TodoListUserRoles.Any(lur => lur.UserId == userId))
            .Include(tl => tl.TodoTasks)
                .ThenInclude(tt => tt.Status)
            .Include(tl => tl.ListOwner)
        .Include(tl => tl.TodoListUserRoles)
            .ThenInclude(tlur => tlur.ListRole)
            .AsSplitQuery()
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets the list of <see cref="TodoList"/> entities that the user has access to.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="rowCount">The number of rows per page.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <see cref="IReadOnlyList{TodoList}"/>.
    /// </returns>
    public async Task<IReadOnlyList<TodoList>> GetAllByUserAsync(int userId, int pageNumber, int rowCount)
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
            .Where(l => l.OwnerId == userId || l.TodoListUserRoles.Any(lur => lur.UserId == userId))
            .Include(tl => tl.TodoTasks)
                .ThenInclude(tt => tt.Status)
            .Include(tl => tl.ListOwner)
        .Include(tl => tl.TodoListUserRoles)
            .ThenInclude(tlur => tlur.ListRole)
            .AsSplitQuery()
            .Skip((pageNumber - 1) * rowCount)
            .Take(rowCount)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a <see cref="TodoList"/> entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>
    /// The <see cref="TodoList"/> entity with the specified identifier,
    /// or <c>null</c> if no such entity exists.
    /// </returns>
    public async Task<TodoList?> GetByIdAsync(int id)
    {
        return await this.dbSet
            .Include(tl => tl.TodoTasks)
                .ThenInclude(tt => tt.Status)
            .Include(tl => tl.ListOwner)
            .Include(tl => tl.TodoListUserRoles)
                .ThenInclude(tlu => tlu.ListRole)
                .AsSplitQuery()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Updates an existing <see cref="TodoList"/> entity in the repository.
    /// </summary>
    /// <param name="entity">The <see cref="TodoList"/> entity to update.</param>
    /// <returns>Updated <see cref="TodoList"/> entity.</returns>
    public async Task<TodoList?> UpdateAsync(TodoList entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var existing = await this.dbSet.FindAsync(entity.Id);
        if (existing is null)
        {
            return null;
        }

        this.Context.Entry(existing).CurrentValues.SetValues(entity);
        _ = await this.Context.SaveChangesAsync();

        await this.Context.Entry(existing)
        .Reference(tl => tl.ListOwner)
        .LoadAsync();

        return existing;
    }
}
