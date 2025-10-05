namespace TodoListApp.WebApi.Models.Dtos.Read;

/// <summary>
/// Data transfer object for a tag.
/// </summary>
public class TagDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TagDto"/> class.
    /// </summary>
    public TagDto()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TagDto"/> class with parameters.
    /// </summary>
    /// <param name="id">The tag unqiue identifier.</param>
    /// <param name="title">The tag title.</param>
    public TagDto(int id, string title)
    {
        this.Id = id;
        this.Title = title;
    }

    /// <summary>
    /// Gets or sets the unique identifier of the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the tag.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}
