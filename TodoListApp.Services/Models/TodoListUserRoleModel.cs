namespace TodoListApp.Services.Models;

/// <summary>
/// Model representing a user's role in a to-do list.
/// </summary>
public class TodoListUserRoleModel : AbstractModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListUserRoleModel"/> class.
    /// </summary>
    /// <param name="id">The list user role ID.</param>
    /// <param name="roleId">The role ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="roleName">The user list role name.</param>
    public TodoListUserRoleModel(int id, int roleId, int userId, string roleName)
        : base(id)
    {
        this.Id = id;
        this.RoleId = roleId;
        this.UserId = userId;
        this.RoleName = roleName;
    }

    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the user list role name.
    /// </summary>
    public string RoleName { get; set; }
}
