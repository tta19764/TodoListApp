using System.ComponentModel.DataAnnotations;
using TodoListApp.Services.WebApi.Enums;

namespace TodoListApp.WebApp.Models.Task;

/// <summary>
/// View model for editing an existing task.
/// </summary>
public class EditTodoTaskViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    [Required]
    public int TaskId { get; set; }

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
    /// Gets or sets the task due date.
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
    /// Gets or sets the priority of the task.
    /// </summary>
    [Required]
    public int ListId { get; set; }

    /// <summary>
    /// Gets or sets the role of the current user (e.g., "Admin", "User").
    /// </summary>
    public Uri ReturnUrl { get; set; } = null!;
}
