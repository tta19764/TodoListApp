namespace TodoListApp.Services.Models;
public class TodoListUserRoleModel : AbstractModel
{
    public TodoListUserRoleModel(int id, int roleId, int userId, string roleName)
        : base(id)
    {
        this.Id = id;
        this.RoleId = roleId;
        this.UserId = userId;
        this.RoleName = roleName;
    }

    public int RoleId { get; set; }

    public int UserId { get; set; }

    public string RoleName { get; set; }
}
