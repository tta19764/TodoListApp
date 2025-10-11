namespace TodoListApp.WebApi.Models.Dtos.Read;

/// <summary>
/// Data transfer object for a to-do list.
/// </summary>
public class TodoListDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDto"/> class.
    /// </summary>
    public TodoListDto()
    {
    }

    /// <summary>
    /// Gets or sets the unique identifier of the list.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the list.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the name of the list owner.
    /// </summary>
    public string OwnerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role of the user in the list.
    /// </summary>
    public string ListRole { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of pending tasks in the list.
    /// </summary>
    public int PendingTasks { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the list owner.
    /// </summary>
    public int ListOwnerId { get; set; }
}
