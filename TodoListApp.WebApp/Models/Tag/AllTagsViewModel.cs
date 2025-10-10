namespace TodoListApp.WebApp.Models.Tag;

/// <summary>
/// View model for displaying all tags with pagination.
/// </summary>
public class AllTagsViewModel
{
    /// <summary>
    /// Gets or sets the collection of tags to display.
    /// </summary>
    public IEnumerable<TagViewModel> Tags { get; set; } = Enumerable.Empty<TagViewModel>();

    /// <summary>
    /// Gets or sets the tag page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of rows to display per tag page.
    /// </summary>
    public int RowCount { get; set; } = 20;

    /// <summary>
    /// Gets or sets the total number of tag pages available based on the current pagination settings.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of tags.
    /// </summary>
    public int TotalTags { get; set; }
}
