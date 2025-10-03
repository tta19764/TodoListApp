namespace TodoListApp.WebApp.Models.List;

/// <summary>
/// View model representing a to-do list with its details and pending tasks.
/// </summary>
public class TodoListViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the list.
    /// </summary>
    public int ListId { get; set; }

    /// <summary>
    /// Gets or sets the title of the list.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the list.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the name of the list owner.
    /// </summary>
    public string UserRole { get; set; } = null!;

    /// <summary>
    /// Gets or sets the number of not completed tasks.
    /// </summary>
    public int PendingTasks { get; set; }
}
