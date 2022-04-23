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
            .Include(s => s.Genres)
            .Include(s => s.Tickets)
            .ToList();
    }

    public Show? FindWithData(int id)
    {
        return _context.Set<Show>()
            .Where(s => s.Id == id)
            .Include(s => s.Author)
            .Include(s => s.Genres)
            .Include(s => s.Tickets)
            .FirstOrDefault();
    }

    public IEnumerable<Show> FilterWithData(Func<Show, bool> filter, int offset, int amount)
    {
        var q = _context.Set<Show>()
            .Where(s => filter(s))
            .Skip(offset);

        if (amount > 0)
        {
            q = q.Take(amount);
        }

        return q.Include(s => s.Author)
            .Include(s => s.Genres)
            .Include(s => s.Tickets)
            .ToList();
    }

    public IEnumerable<Show> GetPageWithData(int offset, int amount)
    {
        return _context.Set<Show>()
            .Skip(offset).Take(amount)
            .Include(s => s.Author)
            .Include(s => s.Genres)
            .Include(s => s.Tickets)
            .ToList();
    }
}