using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Database.Entities;
using TodoListApp.Services.Database.Services;
using TodoListApp.Services.Interfaces.Servicies;
using TodoListApp.WebApi.Servicies;

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

        _ = services.AddScoped<ITodoListService, TodoListService>();

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

        _ = services.AddTransient<IAuthService, AuthService>();

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
                    Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!)),
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = ctx =>
                {
                    Console.WriteLine("JWT validation failed: " + ctx.Exception.Message);
                    return Task.CompletedTask;
                },
            };
        });

        _ = services.AddAuthorization();

        return services;
    }
}
