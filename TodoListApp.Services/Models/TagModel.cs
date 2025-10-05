namespace TodoListApp.Services.Models;
public class TagModel : AbstractModel
{
    public TagModel(int id, string title)
        : base(id)
    {
        this.Title = title;
    }

    public string Title { get; set; }
}
