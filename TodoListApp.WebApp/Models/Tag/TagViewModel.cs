namespace TodoListApp.WebApp.Models.Tag;

/// <summary>
/// ViewModel representing a tag.
/// </summary>
public class TagViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the tag.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}
