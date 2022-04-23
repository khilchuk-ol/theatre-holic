using Microsoft.EntityFrameworkCore;
using TheatreHolic.Data.Models;

namespace TheatreHolic.Data.Repository.Impl;

public class TicketRepository : Repository<int, Ticket>, ITicketRepository
{
    public TicketRepository(DbContext context) : base(context)
    {
    }

    public IEnumerable<Ticket> FindAllWithData()
    {
        return _context.Set<Ticket>()
            .Include(t => t.Show)
            .ToList();
    }

    public Ticket? FindWithData(int id)
    {
        return _context.Set<Ticket>()
            .Where(t => t.Id == id)
            .Include(t => t.Show)
            .FirstOrDefault();
    }

    public IEnumerable<Ticket> FilterWithData(Func<Ticket, bool> filter, int offset, int amount)
    {
        var q = _context.Set<Ticket>()
            .Where(t => filter(t))
            .Skip(offset);

        if (amount > 0)
        {
            q = q.Take(amount);
        }

        return q.Include(t => t.Show)
            .ToList();
    }

    public IEnumerable<Ticket> GetPageWithData(int offset, int amount)
    {
        return _context.Set<Ticket>()
            .Skip(offset).Take(amount)
            .Include(t => t.Show)
            .ToList();
    }
}