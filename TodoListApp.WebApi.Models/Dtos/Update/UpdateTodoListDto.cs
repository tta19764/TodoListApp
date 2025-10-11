namespace TodoListApp.WebApi.Models.Dtos.Update;

/// <summary>
/// Data Transfer Object for updating a to-do list.
/// </summary>
public class UpdateTodoListDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTodoListDto"/> class.
    /// </summary>
    public UpdateTodoListDto()
    {
    }

    /// <summary>
    /// Gets or sets the unique identifier of the list.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the list.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the owner.
    /// </summary>
    public int OwnerId { get; set; }
}
