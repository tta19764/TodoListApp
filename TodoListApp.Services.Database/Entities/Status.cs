namespace TodoListApp.Services.Database.Entities;

/// <summary>
/// Represents a status entity in the database.
/// </summary>
public class Status : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Status"/> class.
    /// </summary>
    public Status()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Status"/> class with parameters.
    /// </summary>
    /// <param name="id">THe unique identifier of the staus.</param>
    /// <param name="statusTitle">The status title.</param>
    public Status(int id, string statusTitle)
        : base(id)
    {
        this.StatusTitle = statusTitle;
    }

    /// <summary>
    /// Gets or sets the title of the status.
    /// </summary>
    public string StatusTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets the task associated with the status.
    /// </summary>
    public virtual IList<TodoTask> TodoTasks { get; private set; } = new List<TodoTask>();
}
