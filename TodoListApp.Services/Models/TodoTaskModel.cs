namespace TodoListApp.Services.Models;

/// <summary>
/// Represents a to-do task with its details, status, owner, tags, and comments.
/// </summary>
public class TodoTaskModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTaskModel"/> class.
    /// </summary>
    /// <param name="Id">The task ID.</param>
    public TodoTaskModel(int id)
        : base(id)
    {
    }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date of task creation.
    /// </summary>
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the task due date.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the task status.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the task owner.
    /// </summary>
    public int OwnerUserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the list.
    /// </summary>
    public int ListId { get; set; }

    /// <summary>
    /// Gets or sets the task owner.
    /// </summary>
    public UserModel? OwnerUser { get; set; }

    /// <summary>
    /// Gets or sets the task status.
    /// </summary>
    public StatusModel? Status { get; set; }

    /// <summary>
    /// Gets or sets the tags associated with the task.
    /// </summary>
    public IEnumerable<TagModel> UsersTags { get; set; } = new List<TagModel>();

    /// <summary>
    /// Gets or sets the comments associated with the task.
    /// </summary>
    public IEnumerable<CommentModel> UserComments { get; set; } = new List<CommentModel>();
}
