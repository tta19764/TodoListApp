namespace TodoListApp.Services.Models;
public class UserModel : AbstractModel
{
    public UserModel(int id, string firstName, string lastName)
        : base(id)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    public string LastName { get; set; }
}
