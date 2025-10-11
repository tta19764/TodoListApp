namespace TodoListApp.WebApi.Models.Dtos.Update;

/// <summary>
/// Data transfer object for updating an existing tag.
/// </summary>
public record UpdateTagDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTagDto"/> class.
    /// </summary>
    public UpdateTagDto()
    {
    }

    /// <summary>
    /// Gets or initializes the unique identifier of the tag.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or initializes the tag title/label.
    /// </summary>
    public string Title { get; init; } = string.Empty;
}
