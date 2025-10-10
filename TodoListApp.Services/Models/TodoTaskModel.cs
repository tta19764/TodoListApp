using System.Collections.ObjectModel;

namespace TodoListApp.Services.Models;

/// <summary>
/// Represents a to-do task with its details, status, owner, tags, and comments.
/// </summary>
public class TodoTaskModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTaskModel"/> class.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="title">The task title.</param>
    /// <param name="description">The task description.</param>
    /// <param name="creationDate">The task creation date.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <param name="statusId">The task status ID.</param>
    /// <param name="ownerUserId">The task assignee ID.</param>
    /// <param name="listId">The list ID.</param>
    /// <param name="owner">The task owner model.</param>
    /// <param name="status">The status model.</param>
    /// <param name="tags">The list with tag models.</param>
    /// <param name="comments">The list with comment models.</param>
    public TodoTaskModel(int id, string title, string? description, DateTime? creationDate, DateTime dueDate, int statusId, int ownerUserId, int listId, UserModel? owner = null, StatusModel? status = null, ReadOnlyCollection<TagModel>? tags = null, ReadOnlyCollection<CommentModel>? comments = null)
        : base(id)
    {
        this.Title = title;
        this.Description = description;
        this.CreationDate = creationDate;
        this.DueDate = dueDate;
        this.StatusId = statusId;
        this.OwnerUserId = ownerUserId;
        this.ListId = listId;
        this.OwnerUser = owner;
        this.Status = status;
        if (tags != null)
        {
            foreach (var tag in tags)
            {
                this.UsersTags.Add(tag);
            }
        }

        if (comments != null)
        {
            foreach (var comment in comments)
            {
                this.UserComments.Add(comment);
            }
        }
    }

    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    public string Title { get; set; }

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
    /// Gets user's tags.
    /// </summary>
    public IList<TagModel> UsersTags { get; private set; } = new List<TagModel>();

    /// <summary>
    /// Gets user's tags.
    /// </summary>
    public IList<CommentModel> UserComments { get; private set; } = new List<CommentModel>();
}
