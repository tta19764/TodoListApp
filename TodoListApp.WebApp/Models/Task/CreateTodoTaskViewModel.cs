using System.ComponentModel.DataAnnotations;
using TodoListApp.Services.WebApi.Enums;

namespace TodoListApp.WebApp.Models.Task;

/// <summary>
/// View model for creating a new task.
/// </summary>
public class CreateTodoTaskViewModel
{
    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    [Display(Name = "Title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the due date of the task.
    /// </summary>
    [Required(ErrorMessage = "Due date is required.")]
    [Display(Name = "Due Date")]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the task.
    /// </summary>
    [Required(ErrorMessage = "Status is required.")]
    [Display(Name = "Status")]
    public TodoTaskStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the ID of the to-do list to which the task belongs.
    /// </summary>
    [Required]
    public int ListId { get; set; }

    /// <summary>
    /// Gets or sets the title of the to-do list to which the task belongs.
    /// </summary>
    public string? ListTitle { get; set; }

    /// <summary>
    /// Gets or sets the return URL after creating the task.
    /// </summary>
    public Uri ReturnUrl { get; set; } = null!;
}
