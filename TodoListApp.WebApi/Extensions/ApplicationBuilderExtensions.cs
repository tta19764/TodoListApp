using TodoListApp.WebApi.Seed;

namespace TodoListApp.WebApi.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static WebApplication UseTodoListAppMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI();
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        return app;
    }

    public static WebApplication MapTodoListAppRoutes(this WebApplication app)
    {
        _ = app.MapControllers();

        return app;
    }

    public static WebApplication SeedTodoListIdentity(this WebApplication app)
    {
        _ = IdentitySeed.EnsurePopulated(app);

        return app;
    }
}
