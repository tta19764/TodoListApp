namespace TodoListApp.Services.Models;
public class CommentModel : AbstractModel
{
    public CommentModel(int id, string text, int taskId, int userId)
        : base(id)
    {
        this.Text = text;
        this.TaskId = taskId;
        this.UserId = userId;
    }

    /// <summary>
    /// Gets or sets the task comment text.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the uique identifier of the task.
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the comment author.
    /// </summary>
    public int UserId { get; set; }
}
