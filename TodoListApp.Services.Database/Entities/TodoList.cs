namespace TodoListApp.Services.Database.Entities;

/// <summary>
/// /// Represents a task list entity int the database.
/// </summary>
public class TodoList : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoList"/> class.
    /// </summary>
    public TodoList()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoList"/> class with parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the list.</param>
    /// <param name="title">The title of the list.</param>
    /// <param name="description">The description of the list.</param>
    public TodoList(int id, int ownerId, string title, string description)
        : base(id)
    {
        this.OwnerId = ownerId;
        this.Title = title;
        this.Description = description;
    }

    /// <summary>
    /// Gets or sets the unqiue identifier of the list owner.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the title of the list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the list.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list owner.
    /// </summary>
    public User ListOwner { get; set; } = null!;

    /// <summary>
    /// Gets the list of tasks.
    /// </summary>
    public virtual IList<TodoTask> TodoTasks { get; private set; } = new List<TodoTask>();

    /// <summary>
    /// Gets the list with list user roles.
    /// </summary>
    public virtual IList<TodoListUserRole> TodoListUserRoles { get; private set; } = new List<TodoListUserRole>();
}
