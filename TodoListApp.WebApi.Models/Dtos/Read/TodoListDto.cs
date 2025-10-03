namespace TodoListApp.WebApi.Models.Dtos.Read;

/// <summary>
/// Data transfer object for a to-do list.
/// </summary>
public class TodoListDto
{
    public TodoListDto()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDto"/> class.
    /// </summary>
    /// <param name="id">The unqiue identifier of the lsit.</param>
    /// <param name="title">The list title.</param>
    /// <param name="description">The list description.</param>
    /// <param name="ownerName">The name of the list owner.</param>
    /// <param name="listRole">The list rol of the user.</param>
    /// <param name="pendingTasks">The number of pending tasks.</param>
    public TodoListDto(int id, string title, string? description, string ownerName, string listRole, int listOwnerId, int pendingTasks = 0)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.OwnerName = ownerName;
        this.ListRole = listRole;
        this.PendingTasks = pendingTasks;
        this.ListOwnerId = listOwnerId;
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
