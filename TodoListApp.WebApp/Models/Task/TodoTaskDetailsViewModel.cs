using TodoListApp.Services.Enums;
using TodoListApp.WebApp.Models.Comment;
using TodoListApp.WebApp.Models.Tag;

namespace TodoListApp.WebApp.Models.Task;

/// <summary>
/// View model for displaying task details.
/// </summary>
public class TodoTaskDetailsViewModel
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
    /// Gets or sets the status ID of the task.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Gets or sets the name of the task owner.
    /// </summary>
    public string OwnerName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the tags associated with the task.
    /// </summary>
    public IEnumerable<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

    /// <summary>
    /// Gets or sets the comments associated with the task.
    /// </summary>
    public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

    /// <summary>
    /// Gets or sets the ID of the list to which the task belongs.
    /// </summary>
    public int? ListId { get; set; }

    /// <summary>
    /// Gets or sets the role of the user in relation to the list.
    /// </summary>
    public ListRole Role { get; set; } = ListRole.None;

    /// <summary>
    /// Gets or sets the return URL to navigate back after viewing the task details.
    /// </summary>
    public Uri ReturnUrl { get; set; } = null!;
}
