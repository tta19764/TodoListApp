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
    /// Initializes a new instance of the <see cref="UpdateTodoListDto"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the list.</param>
    /// <param name="title">The list's title.</param>
    /// <param name="description">The list's description.</param>
    /// <param name="ownerId">The unique identifier of the owner.</param>
    public UpdateTodoListDto(int id, string title, string? description, int ownerId)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.OwnerId = ownerId;
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
