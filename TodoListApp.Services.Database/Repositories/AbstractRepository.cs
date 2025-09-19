using TodoListApp.Services.Database.Data;

namespace TodoListApp.Services.Database.Repositories;

/// <summary>
/// Base for other repositories.
/// </summary>
public abstract class AbstractRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractRepository"/> class with the database context.
    /// </summary>
    /// <param name="context">To-do list app database context.</param>
    protected AbstractRepository(TodoListDbContext context)
    {
        this.Context = context;
    }

    /// <summary>
    /// Gets the database context.
    /// </summary>
    protected TodoListDbContext Context { get; }
}
