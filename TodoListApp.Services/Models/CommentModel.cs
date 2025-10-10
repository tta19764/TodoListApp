namespace TodoListApp.Services.Models;

/// <summary>
/// Model representing a comment on a task.
/// </summary>
public class CommentModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommentModel"/> class.
    /// </summary>
    /// <param name="id">The comment ID.</param>
    /// <param name="text">The comment text.</param>
    /// <param name="taskId">The tas kID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="userModel">The commnet author user model.</param>
    public CommentModel(int id, string text, int taskId, int userId, UserModel? userModel = null)
        : base(id)
    {
        this.Text = text;
        this.TaskId = taskId;
        this.UserId = userId;
        this.User = userModel;
    }

    /// <summary>
    /// Gets or sets the task comment text.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the uique identifier of the task.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the comment author.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the task associated with the comment.
    /// </summary>
    public UserModel? User { get; set; }
}
