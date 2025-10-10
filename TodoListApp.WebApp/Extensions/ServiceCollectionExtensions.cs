using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Email;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.Services.WebApi.Servicies;
using TodoListApp.WebApp.Data;
using TodoListApp.WebApp.Handlers;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Extensions;

/// <summary>
/// Extension methods for configuring services in the dependency injection container.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures and adds application services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app configuration.</param>
    /// <returns>The updated service collection.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the connection string is not found.</exception>
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

        var emailConfig = configuration
        .GetSection("EmailConfiguration")
        .Get<EmailSender>();

        _ = services
            .AddSingleton(emailConfig)
            .AddScoped<IViewRenderService, ViewRenderService>()
            .AddTransient<JwtTokenHandler>()
            .AddTransient<ITokenStorageService, IdentityTokenStorageService>()
            .AddTransient<IEmailService, EmailService>();

        _ = services.AddControllersWithViews(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        });

        _ = services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/Login";
        });

        _ = services.AddHttpClient<IAuthService, AuthApiService>((provider, client) =>
        {
            var apiUrl = configuration["ApiSettings:ApiBaseUrl"];
            var controllerUrl = configuration["ApiSettings:ApiAuthController"];
            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                client.BaseAddress = new Uri(apiUrl + controllerUrl);
            }

            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        });

        _ = services.AddHttpClient<ITodoListService, TodoListApiService>((provider, client) =>
        {
            var apiUrl = configuration["ApiSettings:ApiBaseUrl"];
            var controllerUrl = configuration["ApiSettings:ApiTodoListsController"];
            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                client.BaseAddress = new Uri(apiUrl + controllerUrl);
            }

            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddHttpMessageHandler<JwtTokenHandler>();

        _ = services.AddHttpClient<ITodoTaskService, TodoTaskApiService>((provider, client) =>
        {
            var apiUrl = configuration["ApiSettings:ApiBaseUrl"];
            var controllerUrl = configuration["ApiSettings:ApiTodoTasksController"];
            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                client.BaseAddress = new Uri(apiUrl + controllerUrl);
            }

            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddHttpMessageHandler<JwtTokenHandler>();

        _ = services.AddHttpClient<ISearchTasksService, SearchTasksApiService>((provider, client) =>
        {
            var apiUrl = configuration["ApiSettings:ApiBaseUrl"];
            var controllerUrl = configuration["ApiSettings:ApiSearchTasksController"];
            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                client.BaseAddress = new Uri(apiUrl + controllerUrl);
            }

            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddHttpMessageHandler<JwtTokenHandler>();

        _ = services.AddHttpClient<IAssignedTasksService, AssignedTasksApiService>((provider, client) =>
        {
            var apiUrl = configuration["ApiSettings:ApiBaseUrl"];
            var controllerUrl = configuration["ApiSettings:ApiAssignedTasksController"];
            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                client.BaseAddress = new Uri(apiUrl + controllerUrl);
            }

            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddHttpMessageHandler<JwtTokenHandler>();

        _ = services.AddHttpClient<ITagService, TagApiService>((provider, client) =>
        {
            var apiUrl = configuration["ApiSettings:ApiBaseUrl"];
            var controllerUrl = configuration["ApiSettings:ApiTagsController"];
            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                client.BaseAddress = new Uri(apiUrl + controllerUrl);
            }

            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddHttpMessageHandler<JwtTokenHandler>();

        _ = services.AddHttpClient<ITaskTagService, TaskTagApiService>((provider, client) =>
        {
            var apiUrl = configuration["ApiSettings:ApiBaseUrl"];
            var controllerUrl = configuration["ApiSettings:ApiTaskTagsController"];
            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                client.BaseAddress = new Uri(apiUrl + controllerUrl);
            }

            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddHttpMessageHandler<JwtTokenHandler>();

        _ = services.AddHttpContextAccessor();

        return services;
    }
}
