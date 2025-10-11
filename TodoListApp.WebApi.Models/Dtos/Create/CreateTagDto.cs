namespace TodoListApp.WebApi.Models.Dtos.Create;

/// <summary>
/// Data transfer object for creating a new tag.
/// </summary>
public record CreateTagDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTagDto"/> class.
    /// </summary>
    public CreateTagDto()
    {
    }

    /// <summary>
    /// Gets or initializes the tag title/label.
    /// </summary>
    public string Title { get; init; } = string.Empty;
}
