using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.List;

/// <summary>
/// View model for editing an existing to-do list.
/// </summary>
public class EditTodoListViewModel
{
    /// <summary>
    /// Gets or sets the ID of the to-do list.
    /// </summary>
    public int ListId { get; set; }

    /// <summary>
    /// Gets or sets the title of the to-do list.
    /// </summary>
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the to-do list.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the role of the current user (e.g., "Admin", "User").
    /// </summary>
    public string? UserRole { get; set; }

    /// <summary>
    /// Gets or sets the number of pending tasks in the to-do list.
    /// </summary>
    public int PendingTasks { get; set; }

    /// <summary>
    /// Gets or sets the return URL after editing the to-do list.
    /// </summary>
    public Uri ReturnUrl { get; set; } = null!;
}
