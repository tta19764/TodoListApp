namespace TodoListApp.WebApp.Models.Comment;

/// <summary>
/// View model for editing a comment.
/// </summary>
public class EditCommentViewModel
{
    /// <summary>
    /// Gets or sets the comment ID.
    /// </summary>
    public int CommentId { get; set; }

    /// <summary>
    /// Gets or sets the task ID associated with the comment.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the title of the task associated with the comment.
    /// </summary>
    public string TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the comment text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the return URL after editing the comment.
    /// </summary>
    public Uri ReturnUrl { get; set; } = new Uri("~/", UriKind.Relative);
}
