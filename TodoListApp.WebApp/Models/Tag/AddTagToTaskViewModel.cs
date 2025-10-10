namespace TodoListApp.WebApp.Models.Tag;

/// <summary>
/// ViewModel for adding a tag to a task.
/// </summary>
public class AddTagToTaskViewModel
{
    /// <summary>
    /// Gets or sets the ID of the task to which a tag is being added.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the title of the task to which a tag is being added.
    /// </summary>
    public string TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the selected tag to add to the task.
    /// </summary>
    public int? SelectedTagId { get; set; }

    /// <summary>
    /// Gets or sets the collection of available tags that can be added to the task.
    /// </summary>
    public IEnumerable<TagViewModel> AvailableTags { get; set; } = Enumerable.Empty<TagViewModel>();

    /// <summary>
    /// Gets or sets the return URL to navigate back after adding the tag.
    /// </summary>
    public Uri ReturnUrl { get; set; } = new Uri("~/", UriKind.Relative);
}
