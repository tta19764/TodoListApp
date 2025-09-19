using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;

namespace TodoListApp.WebApi.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTodoListAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<TodoListDbContext>(options =>
        {
            _ = options.UseSqlServer(
                configuration.GetConnectionString("TodoListAppConnection"),
                b => b.MigrationsAssembly("TodoListApp.WebApi"));
        });

        _ = services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<TodoListDbContext>()
            .AddDefaultTokenProviders();

        _ = services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle.
        _ = services.AddEndpointsApiExplorer();
        _ = services.AddSwaggerGen();

        return services;
    }
}
