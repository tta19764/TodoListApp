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
    /// Initializes a new instance of the <see cref="UpdateCommentDto"/> class.
    /// </summary>
    /// <param name="id">The comment ID.</param>
    /// <param name="text">The comment text.</param>
    /// <param name="taskId">The task ID.</param>
    /// <param name="userId">The user ID.</param>
    public UpdateCommentDto(int id, string text)
    {
        this.Id = id;
        this.Text = text;
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
