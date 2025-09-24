namespace TodoListApp.Services.WebApi.Interfaces
{
    public interface IUserService
    {
        Task<string?> Login(string login, string password, Uri? endpoint = null);
    }
}
