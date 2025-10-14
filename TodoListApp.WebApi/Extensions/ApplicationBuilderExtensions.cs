using Microsoft.AspNetCore.Diagnostics;
using TodoListApp.WebApi.CustomLogs;
using TodoListApp.WebApi.Seed;

namespace TodoListApp.WebApi.Extensions;

/// <summary>
/// Extension methods for configuring the application's request pipeline.
/// </summary>
internal static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configures middleware for the TodoList application.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param></param>
    /// <returns>The configured <see cref="WebApplication"/> instance.</returns>
    public static WebApplication UseTodoListAppMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        _ = app.UseSwagger();
        _ = app.UseSwaggerUI();

        if (!app.Environment.IsDevelopment())
        {
            _ = app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionFeature != null)
                    {
                        var logger = context.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("GlobalExceptionHandler");

                        GlobalExceptionLog.LogUnhandledException(
                            logger,
                            exceptionFeature.Error.Message,
                            exceptionFeature.Path);

                        await context.Response.WriteAsJsonAsync(new
                        {
                            error = "An unexpected error occurred while processing your request.",
                            path = exceptionFeature.Path,
                        });
                    }
                });
            });
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        return app;
    }

    /// <summary>
    /// Maps the routes for the TodoList application.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param></param>
    /// <returns>The configured <see cref="WebApplication"/> instance.</returns>
    public static WebApplication MapTodoListAppRoutes(this WebApplication app)
    {
        _ = app.MapControllers();

        return app;
    }

    /// <summary>
    /// Seeds the data for the TodoList application.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param></param>
    /// <returns>The configured <see cref="WebApplication"/> instance.</returns>
    public static WebApplication SeedTodoList(this WebApplication app)
    {
        _ = IdentitySeed.EnsurePopulated(app);
        _ = TagSeed.EnsurePopulated(app);

        return app;
    }
}
