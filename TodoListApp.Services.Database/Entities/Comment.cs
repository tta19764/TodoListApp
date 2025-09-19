namespace TodoListApp.Services.Database.Entities;

/// <summary>
/// Represents a comment entity int the database.
/// </summary>
public class Comment : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Comment"/> class.
    /// </summary>
    public Comment()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Comment"/> class with parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the comment.</param>
    /// <param name="text">The comment text.</param>
    /// <param name="userId">The unique identifier of the comment author.</param>
    /// <param name="taskId">The unique identifier of the task.</param>
    public Comment(int id, string text, int userId, int taskId)
        : base(id)
    {
        this.UserId = userId;
        this.Text = text;
        this.TaskId = taskId;
    }

    /// <summary>
    /// Gets or sets the task comment text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the uique identifier of the task.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the comment author.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the task.
    /// </summary>
    public TodoTask Task { get; set; } = null!;

    /// <summary>
    /// Gets or sets the commet author.
    /// </summary>
    public User Author { get; set; } = null!;
}
