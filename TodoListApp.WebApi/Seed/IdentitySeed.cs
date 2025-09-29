using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;

namespace TodoListApp.WebApi.Seed;

/// <summary>
/// Seeds the database with an initial admin user if it does not exist.
/// </summary>
internal static class IdentitySeed
{
    private const string AdminUser = "Admin";
    private const string AdminPassword = "Secret123$";

    /// <summary>
    /// Ensures the database is populated with the admin user.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance to be configured.</param>
    /// <returns>A task that represents the asynchronous operation with the admin user.</returns>
    public static async Task EnsurePopulated(IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<TodoListDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }

        var userManager = services.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByNameAsync(AdminUser);

        if (user is null)
        {
            user = new User(firstName: "AdminFirstName", lastName: "AdminLastName", userName: AdminUser)
            {
                Email = "admin@example.com",
                PhoneNumber = "555-1234",
            };

            _ = await userManager.CreateAsync(user, AdminPassword);
        }
    }
}
