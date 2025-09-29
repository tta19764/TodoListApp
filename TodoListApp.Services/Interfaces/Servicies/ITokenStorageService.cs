namespace TodoListApp.Services.Interfaces.Servicies;
public interface ITokenStorageService
{
    Task<bool> SaveToken(string userId, string token);

    Task<bool> RemoveToken(string userId);

    Task<string?> GetToken(string userId);
}
