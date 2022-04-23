using Microsoft.EntityFrameworkCore;

namespace TheatreHolic.Data.Repository.Impl;

public class Repository<TIdentity, TEntity> : IRepository<TIdentity, TEntity> where TEntity : class
{
    protected DbContext _context;

    public Repository(DbContext context)
    {
        _context = context;
    }

    public void Create(TEntity item)
    {
        _context.Set<TEntity>().Add(item);
    }

    public virtual TEntity? Find(TIdentity id)
    {
        return _context.Set<TEntity>().Find(id);
    }

    public IEnumerable<TEntity> Filter(Func<TEntity, bool> filter, int offset = 0, int amount = 0)
    {
        var q = _context.Set<TEntity>().Where(e => filter(e));

        if (offset > 0)
        {
            q = q.Skip(offset);
        }
        
        if (amount > 0)
        {
            q = q.Take(amount);
        }

        return q.ToList();
    }

    public virtual IEnumerable<TEntity> FindAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    public IEnumerable<TEntity> GetPage(int offset, int amount)
    {
        var q = _context.Set<TEntity>().AsQueryable();

        if (offset > 0)
        {
            q = q.Skip(offset);
        }
        
        if (amount > 0)
        {
            q = q.Take(amount);
        }

        return q.ToList();
    }

    public void Remove(TIdentity id)
    {
        var item = _context.Set<TEntity>().Find(id);
        if (item == null)
        {
            return;
        }
        
        _context.Entry(item).State = EntityState.Deleted;
        _context.SaveChanges();
    }

    public void Update(TEntity item)
    {
        _context.Set<TEntity>().Attach(item);
        _context.Entry(item).State = EntityState.Modified;
        _context.SaveChanges();
    }
}