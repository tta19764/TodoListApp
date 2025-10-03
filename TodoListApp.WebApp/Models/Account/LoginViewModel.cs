using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Login is required.")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Password is reqired.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; } = true;

    public Uri ReturnUrl { get; set; } = new Uri("/", UriKind.Relative);
}
