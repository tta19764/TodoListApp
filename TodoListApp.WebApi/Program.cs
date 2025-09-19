using TodoListApp.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddTodoListAppServices(builder.Configuration);

var app = builder.Build();

// Middleware
_ = app.UseTodoListAppMiddleware();

// Routes
_ = app.MapTodoListAppRoutes();

await app.RunAsync();
