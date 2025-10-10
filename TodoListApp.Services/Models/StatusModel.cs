namespace TodoListApp.Services.Models;

/// <summary>
/// Model representing a status with its ID and title.
/// </summary>
public class StatusModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StatusModel"/> class.
    /// </summary>
    /// <param name="id">The status ID.</param>
    /// <param name="statustitle">The status title.</param>
    public StatusModel(int id, string statustitle)
        : base(id)
    {
        this.StatusTitle = statustitle;
    }

    /// <summary>
    /// Gets or sets the title of the status.
    /// </summary>
    public string StatusTitle { get; set; }
}
