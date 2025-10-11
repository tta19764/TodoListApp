namespace TodoListApp.WebApi.Models.Dtos.Read;

/// <summary>
/// Data transfer object for reading comment information.
/// </summary>
public class CommentDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommentDto"/> class.
    /// </summary>
    public CommentDto()
    {
    }

    /// <summary>
    /// Gets or sets the unique identifier of the comment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the comment text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who created the comment.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the user who created the comment.
    /// </summary>
    public string UserFirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the user who created the comment.
    /// </summary>
    public string UserLastName { get; set; } = string.Empty;
}
