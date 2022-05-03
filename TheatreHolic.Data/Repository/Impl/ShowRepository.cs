using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TheatreHolic.Data.Exceptions;
using TheatreHolic.Data.Models;

namespace TheatreHolic.Data.Repository.Impl;

public class ShowRepository : Repository<int, Show>, IShowRepository
{
    public ShowRepository(DbContext context) : base(context)
    {
    }
    
    public override void Create(Show item)
    {
        if (_context.Set<Genre>().Find(item.GenreId) == null)
        {
            throw new InvalidForeignKeyException("could not create show, genre with such id does not exist",
                $"genre id = {item.GenreId}");
        }

        if (_context.Set<Author>().Find(item.AuthorId) == null)
        {
            throw new InvalidForeignKeyException("could not create show, author with such id does not exist",
                $"author id = {item.AuthorId}");
        }

        item.Author = null;
        item.Genre = null;

        base.Create(item);
    }
    
    public override void Update(Show item)
    {
        if (_context.Set<Genre>().Find(item.GenreId) == null)
        {
            throw new InvalidForeignKeyException("could not create show, genre with such id does not exist",
                $"genre id = {item.GenreId}");
        }

        if (_context.Set<Author>().Find(item.AuthorId) == null)
        {
            throw new InvalidForeignKeyException("could not create show, author with such id does not exist",
                $"author id = {item.AuthorId}");
        }

        item.Author = null;
        item.Genre = null;

        base.Update(item);
    }

    public IEnumerable<Show> FindAllWithData()
    {
        return _context.Set<Show>()
            .Include(s => s.Author)
            .Include(s => s.Genre)
            .ToList();
    }

    public Show? FindWithData(int id)
    {
        return _context.Set<Show>()
            .Where(s => s.Id == id)
            .Include(s => s.Author)
            .Include(s => s.Genre)
            .FirstOrDefault();
    }

    public IEnumerable<Show> FilterWithData(Expression<Func<Show, bool>> filter, int offset, int amount)
    {
        var q = _context.Set<Show>()
            .Where(filter);

        if (offset > 0)
        {
            q = q.Skip(offset);
        }

        if (amount > 0)
        {
            q = q.Take(amount);
        }

        return q.Include(s => s.Author)
            .Include(s => s.Genre)
            .ToList();
    }

    public IEnumerable<Show> GetPageWithData(int offset, int amount)

    {
        var q = _context.Set<Show>().AsQueryable();

        if (offset > 0)
        {
            q = q.Skip(offset);
        }

        if (amount > 0)
        {
            q = q.Take(amount);
        }

        return q
            .Include(s => s.Author)
            .Include(s => s.Genre)
            .ToList();
    }
}