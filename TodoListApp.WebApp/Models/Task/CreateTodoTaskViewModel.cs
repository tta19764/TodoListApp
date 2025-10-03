using System.ComponentModel.DataAnnotations;
using TodoListApp.Services.WebApi.Enums;

namespace TodoListApp.WebApp.Models.Task;

/// <summary>
/// View model for creating a new task.
/// </summary>
public class CreateTodoTaskViewModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    [Display(Name = "Title")]
    public string Title { get; set; } = null!;

    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Due date is required.")]
    [Display(Name = "Due Date")]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [Display(Name = "Status")]
    public TodoTaskStatus Status { get; set; }

    [Required]
    public int ListId { get; set; }

    public string? ListTitle { get; set; }

    public Uri ReturnUrl { get; set; } = null!;
}
