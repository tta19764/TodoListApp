using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Services;
using TodoListApp.Services.Interfaces.Servicies;

namespace TodoListApp.WebApi.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to add application services.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the TodoListApp services to the IServiceCollection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> collection of services.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> set of application properties.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddTodoListAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<TodoListDbContext>(options =>
        {
            _ = options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("TodoListApp.WebApi"));
        });

        _ = services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<TodoListDbContext>()
            .AddDefaultTokenProviders();

        _ = services
            .AddTransient<ITodoListService, TodoListService>()
            .AddTransient<ITodoTaskService, TodoTaskService>()
            .AddTransient<ITagService, TagService>()
            .AddTransient<IAssignedTasksService, AssignedTasksService>()
            .AddTransient<ISearchTasksService, SearchTasksService>()
            .AddTransient<ITaskTagService, TaskTagService>()
            .AddTransient<IAuthService, AuthService>();

        _ = services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle.
        _ = services.AddEndpointsApiExplorer();
        _ = services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoListApp API", Version = "v1" });

            // Add JWT Authentication
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token.\r\n\r\nExample: \"Bearer eyJhbGciOiJI...\"",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    Array.Empty<string>()
                },
            });
        });

        _ = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["AppSettings:Issuer"],
                ValidAudience = configuration["AppSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["AppSettings:Token"] ?? "SomeSuperSecureKey")),
                ClockSkew = TimeSpan.FromMinutes(5),
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = async ctx =>
                {
                    ctx.HandleResponse();

                    var response = ctx.Response;
                    response.StatusCode = 401;
                    response.ContentType = "application/json";

                    string message;
                    string errorCode;

                    if (ctx.AuthenticateFailure is SecurityTokenExpiredException)
                    {
                        message = "Token has expired";
                        errorCode = "TOKEN_EXPIRED";
                        response.Headers.Add("Token-Expired", "true");
                    }
                    else if (ctx.AuthenticateFailure is SecurityTokenInvalidSignatureException)
                    {
                        message = "Invalid token signature";
                        errorCode = "INVALID_SIGNATURE";
                    }
                    else if (ctx.AuthenticateFailure is SecurityTokenValidationException)
                    {
                        message = "Invalid token";
                        errorCode = "INVALID_TOKEN";
                    }
                    else
                    {
                        message = "Authentication failed";
                        errorCode = "AUTH_FAILED";
                    }

                    var result = new
                    {
                        error = errorCode,
                        message = message,
                        timestamp = DateTime.UtcNow,
                    };

                    await response.WriteAsync(JsonSerializer.Serialize(result));
                },
            };
        });

        _ = services.AddAuthorization();

        return services;
    }
}
