using TheatreHolic.Domain.Models;

namespace TheatreHolic.Domain.Services;

public interface ITicketService
{
    void CreateTicket(Ticket item);
    void DeleteTicket(int id);

    bool BookTicket(int ticketId);
    bool BuyTicket(int ticketId);
    void RemoveBooking(int ticketId);
    
    IEnumerable<Ticket> GetAvailableTickets(int showId);
}