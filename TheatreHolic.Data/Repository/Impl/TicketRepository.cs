using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TheatreHolic.Data.Exceptions;
using TheatreHolic.Data.Models;

namespace TheatreHolic.Data.Repository.Impl;

public class TicketRepository : Repository<int, Ticket>, ITicketRepository
{
    public TicketRepository(DbContext context) : base(context)
    {
    }
    
    public override void Create(Ticket item)
    {
        if (_context.Set<Show>().Find(item.ShowId) == null)
        {
            throw new InvalidForeignKeyException("could not create ticket, show with such id does not exist",
                $"show id = {item.ShowId}");
        }

        item.Show = null;

        base.Create(item);
    }
    
    public override void Update(Ticket item)
    {
        if (_context.Set<Show>().Find(item.ShowId) == null)
        {
            throw new InvalidForeignKeyException("could not create ticket, show with such id does not exist",
                $"show id = {item.ShowId}");
        }

        item.Show = null;

        base.Update(item);
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

    public IEnumerable<Ticket> FilterWithData(Expression<Func<Ticket, bool>> filter, int offset, int amount)
    {
        var q = _context.Set<Ticket>()
            .Where(filter);

        if (offset > 0)
        {
            q = q.Skip(offset);
        }

        if (amount > 0)
        {
            q = q.Take(amount);
        }

        return q.Include(t => t.Show)
            .ToList();
    }

    public IEnumerable<Ticket> GetPageWithData(int offset, int amount)
    {
        var q = _context.Set<Ticket>().AsQueryable();

        if (offset > 0)
        {
            q = q.Skip(offset);
        }

        if (amount > 0)
        {
            q = q.Take(amount);
        }

        return q
            .Include(t => t.Show)
            .ToList();
    }
}