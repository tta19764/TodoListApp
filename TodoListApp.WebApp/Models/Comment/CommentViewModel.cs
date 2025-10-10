namespace TodoListApp.WebApp.Models.Comment;

/// <summary>
/// View model for comments.
/// </summary>
public class CommentViewModel
{
    /// <summary>
    /// Gets or sets the comment ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the comment text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the task ID associated with the comment.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the user ID of the comment author.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the username of the comment author.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the current user can edit the comment.
    /// </summary>
    public bool CanEdit { get; set; }
}
