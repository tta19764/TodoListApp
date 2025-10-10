namespace TodoListApp.Services.Enums;

/// <summary>
/// Defines the roles a user can have in relation to a list.
/// </summary>
public enum ListRole
{
    /// <summary>
    /// User has no access to the list..
    /// </summary>
    None = 0,

    /// <summary>
    /// User with full permissions, including managing roles and deleting the list.
    /// </summary>
    Owner = 1,

    /// <summary>
    /// User with permissions to modify list items but cannot manage roles or delete the list.
    /// </summary>
    Editor = 2,

    /// <summary>
    /// User with read-only access to the list.
    /// </summary>
    Viewer = 3,
}
