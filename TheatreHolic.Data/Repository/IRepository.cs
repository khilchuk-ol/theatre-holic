namespace TheatreHolic.Data.Repository;

public interface IRepository<TIdentity, TEntity>
{
    void Create(TEntity item);
    void Update(TEntity item);
    void Remove(TEntity item);
    IEnumerable<TEntity> FindAll();
    TEntity? Find(TIdentity id);
    IEnumerable<TEntity> Filter(Func<TEntity, bool> filter, int offset, int amount);
    IEnumerable<TEntity> GetPage(int offset, int amount);
}