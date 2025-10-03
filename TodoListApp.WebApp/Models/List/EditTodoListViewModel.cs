using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.List;

public class EditTodoListViewModel
{
    public int ListId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    public string? UserRole { get; set; }

    public int PendingTasks { get; set; }

    public Uri ReturnUrl { get; set; } = null!;
}
