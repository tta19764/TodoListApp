using System.Collections.ObjectModel;

namespace TodoListApp.Services.Models;
public class TodoTaskModel : AbstractModel
{
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
