namespace TodoListApp.WebApp.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static WebApplication UseTodoListAppMiddleware(this WebApplication app)
    {
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

    public static WebApplication MapTodoListAppRoutes(this WebApplication app)
    {
        _ = app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        _ = app.MapRazorPages();

        return app;
    }
}
