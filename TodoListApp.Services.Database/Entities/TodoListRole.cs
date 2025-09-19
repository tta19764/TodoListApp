namespace TodoListApp.Services.Database.Entities;

/// <summary>
/// Represents a list role entity int the database.
/// </summary>
public class TodoListRole : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListRole"/> class.
    /// </summary>
    public TodoListRole()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListRole"/> class with parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the role.</param>
    /// <param name="roleName">The role name.</param>
    public TodoListRole(int id, string roleName)
        : base(id)
    {
        this.RoleName = roleName;
    }

    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// Gets the list with list user roles.
    /// </summary>
    public virtual IList<TodoListUserRole> TodoListUserRoles { get; private set; } = new List<TodoListUserRole>();
}
