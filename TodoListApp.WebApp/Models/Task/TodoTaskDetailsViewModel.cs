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

    public List<string> Tags { get; set; } = new List<string>();

    public List<string> Comments { get; set; } = new List<string>();

    public int? ListId { get; set; }

    public Uri ReturnUrl { get; set; } = null!;
}
