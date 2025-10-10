namespace TodoListApp.Services.Models;

/// <summary>
/// Model representing a tag with its ID and title.
/// </summary>
public class TagModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TagModel"/> class.
    /// </summary>
    /// <param name="id">The tag ID.</param>
    /// <param name="title">The tag title.</param>
    public TagModel(int id, string title)
        : base(id)
    {
        this.Title = title;
    }

    /// <summary>
    /// Gets or sets the title of the tag.
    /// </summary>
    public string Title { get; set; }
}
