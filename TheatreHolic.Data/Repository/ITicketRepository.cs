using TheatreHolic.Data.Models;

namespace TheatreHolic.Data.Repository;

public interface ITicketRepository: IRepository<int, Ticket>
{
    IEnumerable<Ticket> FindAllWithData();
    Ticket? FindWithData(int id);
    IEnumerable<Ticket> FilterWithData(Func<Ticket, bool> filter, int offset, int amount);
    IEnumerable<Ticket> GetPageWithData(int offset, int amount);
}