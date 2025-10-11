namespace TodoListApp.WebApi.Models.Dtos.Read;

/// <summary>
/// Represents a data transfer object for status information.
/// </summary>
public class StatusDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StatusDto"/> class.
    /// </summary>
    public StatusDto()
    {
    }

    /// <summary>
    /// Gets or sets the unique identifier of the status.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the status.
    /// </summary>
    public string StatusTitle { get; set; } = string.Empty;
}
