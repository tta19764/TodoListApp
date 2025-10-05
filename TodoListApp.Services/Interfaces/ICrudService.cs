using TodoListApp.Services.Models;

namespace TodoListApp.Services.Interfaces;
public interface ICrudService<TEntity>
    where TEntity : AbstractModel
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();

    Task<IReadOnlyList<TEntity>> GetAllAsync(int pageNumber, int rowCount);

    Task<TEntity> GetByIdAsync(int id);

    Task<TEntity> AddAsync(TEntity model);

    Task DeleteAsync(int id);

    Task<TEntity> UpdateAsync(TEntity model);
}
