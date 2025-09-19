namespace TodoListApp.Services.Database.Interfaces;
public interface IRepository<TEntity>
{
    IEnumerable<TEntity> GetAll();

    IEnumerable<TEntity> GetAll(int pageNumber, int rowCount);

    TEntity? GetById(int id);

    void Add(TEntity entity);

    void Delete(TEntity entity);

    void DeleteById(int id);

    void Update(TEntity entity);
}
