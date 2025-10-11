namespace TodoListApp.WebApi.Models.Dtos.Create;

/// <summary>
/// Data transfer object for creating a new comment.
/// </summary>
public record CreateCommentDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCommentDto"/> class.
    /// </summary>
    public CreateCommentDto()
    {
    }

    /// <summary>
    /// Gets or initializes the comment text.
    /// </summary>
    public string Text { get; init; } = string.Empty;
}
