using TodoListApp.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTodoListAppServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseTodoListAppMiddleware();

app.MapTodoListAppRoutes();

await app.RunAsync();
