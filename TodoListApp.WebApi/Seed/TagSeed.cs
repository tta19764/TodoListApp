using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;

namespace TodoListApp.WebApi.Seed;

internal static class TagSeed
{
    /// <summary>
    /// Ensures the database is populated with base tags.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance to be configured.</param>
    /// <returns>A task that represents the asynchronous operation with the base tags.</returns>
    public static async Task EnsurePopulated(IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<TodoListDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }

        if (await context.Tags.AnyAsync())
        {
            return;
        }

        // Define base tags
        var baseTags = new[]
        {
            new Tag(0, "Work"),
            new Tag(0, "Personal"),
            new Tag(0, "Urgent"),
            new Tag(0, "Important"),
            new Tag(0, "Shopping"),
            new Tag(0, "Health"),
            new Tag(0, "Finance"),
            new Tag(0, "Learning"),
            new Tag(0, "Family"),
            new Tag(0, "Other"),
        };

        // Add tags to database
        await context.Tags.AddRangeAsync(baseTags);
        _ = await context.SaveChangesAsync();
    }
}
