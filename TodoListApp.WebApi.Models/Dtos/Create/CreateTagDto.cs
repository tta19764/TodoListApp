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
    /// Initializes a new instance of the <see cref="CreateTagDto"/> class.
    /// </summary>
    /// <param name="title">The tag title.</param>
    public CreateTagDto(string title)
    {
        this.Title = title;
    }

    /// <summary>
    /// Gets or initializes the tag title/label.
    /// </summary>
    public string Title { get; init; } = string.Empty;
}
