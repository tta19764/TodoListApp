namespace TodoListApp.Services.Models;
public class TodoListModel : AbstractModel
{
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
