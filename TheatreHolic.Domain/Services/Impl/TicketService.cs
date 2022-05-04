using AutoMapper;
using Microsoft.Extensions.Logging;
using TheatreHolic.Data.Exceptions;
using TheatreHolic.Data.Repository;
using TheatreHolic.Domain.Models;
using TicketState = TheatreHolic.Data.Models.TicketState;

namespace TheatreHolic.Domain.Services.Impl;

public class TicketService : ITicketService
{
    private ITicketRepository _repository;
    private IMapper _mapper;
    private readonly ILogger<TicketService> _logger;

    private readonly List<TicketState> BOOKABLE_STATES = new() {TicketState.Available};
    private readonly List<TicketState> BUYABLE_STATES = new() {TicketState.Available, TicketState.Booked};

    public TicketService(ITicketRepository repository, IMapper mapper, ILogger<TicketService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public bool CreateTicket(Ticket item)
    {
        if (item.Price <= 0 || item.Row <= 0 || item.Seat <= 0)
        {
            return false;
        }

        item.State = Models.TicketState.Available;
        
        try
        {
            _repository.Create(_mapper.Map<Ticket, Data.Models.Ticket>(item));
        }
        catch (InvalidForeignKeyException e)
        {
            _logger.Log(LogLevel.Warning, e.ToString());
            return false;
        }

        return true;
        
    }

    public bool DeleteTicket(int id)
    {
        return _repository.Remove(id);
    }

    public bool BookTicket(int ticketId)
    {
        var ticket = _repository.Find(ticketId);
        if (ticket == null || !BOOKABLE_STATES.Contains(ticket.State))
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
        if (ticket == null || !BUYABLE_STATES.Contains(ticket.State))
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
        return _repository.Filter(t => t.State == TicketState.Available && t.ShowId == showId, -1, -1)
            .Select(t => _mapper.Map<Data.Models.Ticket, Ticket>(t))
            .ToList();
    }
}