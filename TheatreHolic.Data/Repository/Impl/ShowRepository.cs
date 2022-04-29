using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TheatreHolic.Data.Models;

namespace TheatreHolic.Data.Repository.Impl;

public class ShowRepository : Repository<int, Show>, IShowRepository
{
    public ShowRepository(DbContext context) : base(context)
    {
    }

    public IEnumerable<Show> FindAllWithData()
    {
        return _context.Set<Show>()
            .Include(s => s.Author)
            .Include(s => s.Genre)
            .Include(s => s.Tickets)
            .ToList();
    }

    public Show? FindWithData(int id)
    {
        return _context.Set<Show>()
            .Where(s => s.Id == id)
            .Include(s => s.Author)
            .Include(s => s.Genre)
            .Include(s => s.Tickets)
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
            .Include(s => s.Tickets)
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
            .Include(s => s.Tickets)
            .ToList();
    }
}