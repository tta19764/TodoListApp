namespace TodoListApp.Services.Database.Entities;

/// <summary>
/// Represents a link between tags and tasks in the database.
/// </summary>
public class TaskTags
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskTags"/> class.
    /// </summary>
    public TaskTags()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskTags"/> class with parameters.
    /// </summary>
    /// <param name="taskId">The unique identifierof the task.</param>
    /// <param name="tagId">The unique identifier of the tag.</param>
    public TaskTags(int taskId, int tagId)
    {
        this.TaskId = taskId;
        this.TagId = tagId;
    }

    /// <summary>
    /// Gets or sets the unqiue identifier of the task.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the tag.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    public Tag Tag { get; set; } = null!;

    /// <summary>
    /// Gets or sets the task.
    /// </summary>
    public TodoTask Task { get; set; } = null!;
}
