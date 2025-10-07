namespace TodoListApp.WebApp.Models.Task;

/// <summary>
/// View model for displaying a to-do task.
/// </summary>
public class TodoTaskViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the task.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the due date of the task.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the task.
    /// </summary>
    public string Status { get; set; } = null!;

    /// <summary>
    /// Gets or sets the owner's name.
    /// </summary>
    public string OwnerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier of the list the task belongs to.
    /// </summary>
    public int ListId { get; set; }

    /// <summary>
    /// Gets or sets the list of tags associated with the task.
    /// </summary>
    public IEnumerable<string> Tags { get; set; } = new List<string>();
}
