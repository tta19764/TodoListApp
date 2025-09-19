using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.WebApi.Interfaces;
using TodoListApp.Services.WebApi.Servicies;
using TodoListApp.WebApp.Data;

namespace TodoListApp.WebApp.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTodoListAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        _ = services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        _ = services.AddDatabaseDeveloperPageExceptionFilter();

        _ = services.AddDefaultIdentity<AppUser>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        _ = services.AddControllersWithViews();

        _ = services.AddHttpClient<IUserService, UserService>((provider, client) =>
        {
            var apiUrl = configuration["ApiSettings:ApiBaseUrl"];
            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                client.BaseAddress = new Uri(apiUrl);
            }

            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });

        _ = services.AddHttpContextAccessor();

        return services;
    }
}
