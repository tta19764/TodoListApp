using TodoListApp.WebApp.Middleware;

namespace TodoListApp.WebApp.Extensions;

/// <summary>
/// Extension methods for configuring the application builder.
/// </summary>
internal static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the middleware for the TodoList application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <returns>The configured application.</returns>
    public static WebApplication UseTodoListAppMiddleware(this WebApplication app)
    {
        _ = app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseMigrationsEndPoint();
        }
        else
        {
            _ = app.UseExceptionHandler("/Home/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _ = app.UseHsts();
        }

        _ = app.UseHttpsRedirection();
        _ = app.UseStaticFiles();

        _ = app.UseRouting();

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        return app;
    }

    /// <summary>
    /// Maps the routes for the TodoList application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <returns>The application with mapped routes.</returns>
    public static WebApplication MapTodoListAppRoutes(this WebApplication app)
    {
        _ = app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        _ = app.MapRazorPages();

        return app;
    }
}
