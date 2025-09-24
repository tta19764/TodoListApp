namespace TodoListApp.Services.Models;
public class TagModel : AbstractModel
{
    public TagModel(int id, string title, int userId, int taskId)
        : base(id)
    {
        this.StatusTitle = title;
        this.UserId = userId;
        this.TaskId = taskId;
    }

    public string StatusTitle { get; set; }

    public int UserId { get; set; }

    public int TaskId { get; set; }
}
