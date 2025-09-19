namespace TodoListApp.Services.Database.Entities;

/// <summary>
/// Represents a task entity int the database.
/// </summary>
public class TodoTask : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTask"/> class.
    /// </summary>
    public TodoTask()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTask"/> class with parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the task.</param>
    /// <param name="title">The title of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="creationDate">The creation date of the task.</param>
    /// <param name="dueDate">The due date of the task.</param>
    /// <param name="statusId">The unique identifier of the task status.</param>
    /// <param name="ownerUserId">The unique identifier of the owner.</param>
    public TodoTask(int id, string title, string description, DateTime creationDate, DateTime dueDate, int statusId, int ownerUserId, int listId)
        : base(id)
    {
        this.Title = title;
        this.Description = description;
        this.CreationDate = creationDate;
        this.DueDate = dueDate;
        this.StatusId = statusId;
        this.OwnerUserId = ownerUserId;
        this.ListId = listId;
    }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date of task creation.
    /// </summary>
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the task due date.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the task status.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the task owner.
    /// </summary>
    public int OwnerUserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the list.
    /// </summary>
    public int ListId { get; set; }

    /// <summary>
    /// Gets or sets the task owner.
    /// </summary>
    public User OwnerUser { get; set; } = null!;

    /// <summary>
    /// Gets or sets the task status.
    /// </summary>
    public Status Status { get; set; } = null!;

    /// <summary>
    /// Gets or sets the list.
    /// </summary>
    public TodoList TodoList { get; set; } = null!;

    /// <summary>
    /// Gets the list of task tags associated with the task.
    /// </summary>
    public virtual IList<TaskTags> TaskTags { get; private set; } = new List<TaskTags>();

    /// <summary>
    /// Gets the list of comments.
    /// </summary>
    public virtual ICollection<Comment> Comments { get; private set; } = new List<Comment>();
}
