using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.CustomLogs;
using TodoListApp.WebApp.Models.Errors;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    public HomeController(ILogger<HomeController> logger)
    {
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        HomeLog.LogIndexPageEntered(this.logger);
        return this.View();
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        HomeLog.LogPrivacyPageEntered(this.logger);
        return this.View();
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        HomeLog.LogErrorPageEntered(this.logger);

        // Try to get error details from HttpContext.Items (set by GlobalExceptionHandler)
        if (this.HttpContext.Items.TryGetValue("ErrorViewModel", out var errorViewModel)
            && errorViewModel is ErrorViewModel model)
        {
            return this.View(model);
        }

        // Fallback to basic error model
        return this.View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            ErrorCode = "ERR_UNKNOWN",
            ErrorMessage = "An unexpected error occurred.",
            StatusCode = this.HttpContext.Response.StatusCode != 200
                ? this.HttpContext.Response.StatusCode
                : 500,
            Timestamp = DateTime.UtcNow,
        });
    }
}
