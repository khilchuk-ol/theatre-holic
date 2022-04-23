using AutoMapper;
using TheatreHolic.Data.Repository;
using TheatreHolic.Domain.Models;
using TicketState = TheatreHolic.Data.Models.TicketState;

namespace TheatreHolic.Domain.Services.Impl;

public class TicketService : ITicketService
{
    private ITicketRepository _repository;
    private IMapper _mapper;

    public TicketService(ITicketRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public void CreateTicket(Ticket item)
    {
        _repository.Create(_mapper.Map<Ticket, Data.Models.Ticket>(item));
    }

    public void DeleteTicket(int id)
    {
        _repository.Remove(id);
    }

    public bool BookTicket(int ticketId)
    {
        var ticket = _repository.Find(ticketId);
        if (ticket == null)
        {
            return false;
        }

        ticket.State = TicketState.Booked;
        _repository.Update(ticket);
        return true;
    }

    public bool BuyTicket(int ticketId)
    {
        var ticket = _repository.Find(ticketId);
        if (ticket == null)
        {
            return false;
        }

        ticket.State = TicketState.Sold;
        _repository.Update(ticket);
        return true;
    }

    public void RemoveBooking(int ticketId)
    {
        var ticket = _repository.Find(ticketId);
        if (ticket == null)
        {
            return;
        }

        ticket.State = TicketState.Available;
        _repository.Update(ticket);
    }

    public IEnumerable<Ticket> GetAvailableTickets(int showId)
    {
        return _repository.Filter(
                t => t.State == TicketState.Available && t.ShowId == showId, -1, -1)
            .Select(t => _mapper.Map<Data.Models.Ticket, Ticket>(t));
    }
}