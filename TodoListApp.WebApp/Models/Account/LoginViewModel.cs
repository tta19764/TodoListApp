using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.Account;

/// <summary>
/// View model for the login form.
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// Gets or sets the user login (username or email).
    /// </summary>
    [Required(ErrorMessage = "Login is required.")]
    public string Login { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user password.
    /// </summary>
    [Required(ErrorMessage = "Password is reqired.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether to remember the user on the device.
    /// </summary>
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; } = true;

    /// <summary>
    /// Gets or sets the return URL after a successful login.
    /// </summary>
    public Uri ReturnUrl { get; set; } = new Uri("/", UriKind.Relative);
}
