using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.List;

/// <summary>
/// View model for creating a new to-do list.
/// </summary>
public class CreateTodoListViewModel
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    public Uri ReturnUrl { get; set; } = null!;
}
