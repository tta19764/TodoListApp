namespace TodoListApp.Services.Database.Entities;

/// <summary>
/// Represents an entity that connects lists with users and their roles.
/// </summary>
public class TodoListUserRole : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListUserRole"/> class.
    /// </summary>
    public TodoListUserRole()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListUserRole"/> class with parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the list user role.</param>
    /// <param name="todoListId">The unique identifier of the list.</param>
    /// <param name="userId">The unique identifer of the user.</param>
    /// <param name="roleId">The unique identifier of the role.</param>
    public TodoListUserRole(int id, int todoListId, int userId, int roleId)
        : base(id)
    {
        this.TodoListId = todoListId;
        this.UserId = userId;
        this.TodoListRoleId = roleId;
    }

    /// <summary>
    /// Gets or sets the list unique identifier of the list.
    /// </summary>
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the list unique identifier of the user.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the list unique identifier of the role.
    /// </summary>
    public int TodoListRoleId { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    public User ListUser { get; set; } = null!;

    /// <summary>
    /// Gets or sets the list.
    /// </summary>
    public TodoList List { get; set; } = null!;

    /// <summary>
    /// Gets or sets the role.
    /// </summary>
    public TodoListRole ListRole { get; set; } = null!;
}
