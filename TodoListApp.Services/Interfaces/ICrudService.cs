using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces;
public interface ICrudService<TEntity>
    where TEntity : AbstractModel
{
    Task<IReadOnlyList<TEntity>> GetAllAsync(int userId);

    Task<IReadOnlyList<TEntity>> GetAllAsync(int userId, int pageNumber, int rowCount);

    Task<TEntity> GetByIdAsync(int userId, int id);

    Task<TEntity> AddAsync(TEntity model);

    Task DeleteAsync(int userId, int id);

    Task<TEntity> UpdateAsync(int userId, TEntity model);
}
