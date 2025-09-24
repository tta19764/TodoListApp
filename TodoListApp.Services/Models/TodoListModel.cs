namespace TodoListApp.Services.Models;
public class TodoListModel : AbstractModel
{
    public TodoListModel(int id, int ownerId, string title, string description, UserModel? listOwner = null, IList<TodoTaskModel>? tasks = null)
        : base(id)
    {
        this.OwnerId = ownerId;
        this.Title = title;
        this.Description = description;
        this.ListOwner = listOwner;
        if (tasks is not null)
        {
            foreach (TodoTaskModel t in tasks)
            {
                this.TodoTasks.Add(t);
            }
        }
    }

    /// <summary>
    /// Gets or sets the unqiue identifier of the list owner.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the title of the list.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the list.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the list owner.
    /// </summary>
    public UserModel? ListOwner { get; set; }

    /// <summary>
    /// Gets the list of tasks.
    /// </summary>
    public virtual IList<TodoTaskModel> TodoTasks { get; private set; } = new List<TodoTaskModel>();
}
