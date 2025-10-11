namespace TodoListApp.WebApi.Models.Dtos.Create;

/// <summary>
/// Data transfer object for creating a new to-do list.
/// </summary>
public class CreateTodoListDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTodoListDto"/> class.
    /// </summary>
    public CreateTodoListDto()
    {
    }

    /// <summary>
    /// Gets or sets the title of the to-do list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the to-do list.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the list owner.
    /// </summary>
    public int OwnerId { get; set; }
}
