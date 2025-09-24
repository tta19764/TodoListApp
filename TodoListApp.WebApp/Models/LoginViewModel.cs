using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Login is required.")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Password is reqired.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

    public Uri ReturnUrl { get; set; } = new Uri("/", UriKind.Relative);
}
