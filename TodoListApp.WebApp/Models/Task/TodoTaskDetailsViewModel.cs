using TodoListApp.Services.Enums;
using TodoListApp.WebApp.Models.Tag;

namespace TodoListApp.WebApp.Models.Task;

/// <summary>
/// View model for displaying task details.
/// </summary>
public class TodoTaskDetailsViewModel
{
    public int TaskId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime DueDate { get; set; }

    public string Status { get; set; } = null!;

    public int StatusId { get; set; }

    public string OwnerName { get; set; } = null!;

    public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

    public List<string> Comments { get; set; } = new List<string>();

    public int? ListId { get; set; }

    /// <summary>
    /// Gets or sets the role of the user in relation to the list.
    /// </summary>
    public ListRole Role { get; set; } = ListRole.None;

    public Uri ReturnUrl { get; set; } = null!;
}
