namespace TodoListApp.Services.Models;

/// <summary>
/// Model representing a to-do list with its details.
/// </summary>
public class TodoListModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListModel"/> class.
    /// </summary>
    /// <param name="id">The list ID.</param>
    /// <param name="ownerId">The list owner ID.</param>
    /// <param name="title">The list title.</param>
    /// <param name="description">The list description.</param>
    /// <param name="activeTasks">The nmber of active tasks in the list.</param>
    /// <param name="ownerFullName">The fillname of the owner.</param>
    /// <param name="userRole">The list user role.</param>
    public TodoListModel(int id, int ownerId, string title, string? description = null, int activeTasks = 0, string? ownerFullName = null, string userRole = "No role")
        : base(id)
    {
        this.OwnerId = ownerId;
        this.Title = title;
        this.Description = description;
        this.OwnerFullName = ownerFullName;
        this.ActiveTasks = activeTasks;
        this.UserRole = userRole;
    }

    /// <summary>
    /// Gets or sets the unqiue identifier of the list owner.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the title of the list.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the list.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the list owner.
    /// </summary>
    public string? OwnerFullName { get; set; }

    /// <summary>
    /// Gets or sets the list user role.
    /// </summary>
    public string UserRole { get; set; }

    /// <summary>
    /// Gets or sets the number of active tasks in the list.
    /// </summary>
    public int ActiveTasks { get; set; }
}
