namespace TodoListApp.Services.Models;
public class StatusModel : AbstractModel
{
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
