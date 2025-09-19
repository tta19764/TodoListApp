using TodoListApp.Services.Database.Entities;

namespace TodoListApp.Services.Database.Interfaces;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();

    Task<IReadOnlyList<TEntity>> GetAllAsync(int pageNumber, int rowCount);

    Task<TEntity?> GetByIdAsync(int id);

    Task<TEntity> AddAsync(TEntity entity);

    Task<bool> DeleteAsync(TEntity entity);

    Task<bool> DeleteByIdAsync(int id);

    Task<TEntity?> UpdateAsync(TEntity entity);
}
