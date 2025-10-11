namespace TodoListApp.WebApi.Models.Dtos.Update;

/// <summary>
/// Data Transfer Object for updating a comment.
/// </summary>
public class UpdateCommentDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCommentDto"/> class.
    /// </summary>
    public UpdateCommentDto()
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
}
