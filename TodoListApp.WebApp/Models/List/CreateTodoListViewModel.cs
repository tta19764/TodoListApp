using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.List;

/// <summary>
/// View model for creating a new to-do list.
/// </summary>
public class CreateTodoListViewModel
{
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
    /// Gets or sets the return URL after creating the to-do list.
    /// </summary>
    public Uri ReturnUrl { get; set; } = null!;
}
