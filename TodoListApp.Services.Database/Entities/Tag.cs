namespace TodoListApp.Services.Database.Entities;

/// <summary>
/// Represents a tag entity in the database.
/// </summary>
public class Tag : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Tag"/> class.
    /// </summary>
    public Tag()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tag"/> class with parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the tag.</param>
    /// <param name="lable">The tag lable.</param>
    /// <param name="userId">The unique identifier of the tag owner.</param>
    public Tag(int id, string lable, int userId)
        : base(id)
    {
        this.Label = lable;
        this.UserId = userId;
    }

    /// <summary>
    /// Gets or sets the tag lable.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the tag owner.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the tag owner.
    /// </summary>
    public User TagAuthor { get; set; } = null!;

    /// <summary>
    /// Gets the list of task tags associated with the tag.
    /// </summary>
    public virtual IList<TaskTags> TaskTags { get; private set; } = new List<TaskTags>();
}
