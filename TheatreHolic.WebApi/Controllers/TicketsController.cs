using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatreHolic.Domain.Services;
using TheatreHolic.WebApi.Dtos.Ticket;

namespace TheatreHolic.WebApi.Controllers;

[ApiController]
public class TicketsController
{
    private readonly ILogger<TicketsController> _logger;
    private readonly ITicketService _service;
    private readonly IMapper _mapper;

    public TicketsController(ILogger<TicketsController> logger, ITicketService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("/theatreholic/api/tickets/show/{id:int}/available")]
    public IResult GetAvailableTickets(int id)
    {
        var tickets = _service.GetAvailableTickets(id).ToList();

        return !tickets.Any()
            ? Results.NotFound()
            : Results.Ok(tickets.Select(t => _mapper.Map<Domain.Models.Ticket, Ticket>(t)).ToList());
    }
    
    [HttpPost]
    [Route("/theatreholic/api/tickets/")]
    public IResult CreateTicket([FromBody] CreateTicketDto dto)
    {
        var ticket = _mapper.Map<CreateTicketDto, Domain.Models.Ticket>(dto);

        return _service.CreateTicket(ticket) ? Results.Ok() : Results.BadRequest();
    }

    [HttpPost]
    [Route("/theatreholic/api/tickets/remove_booking/{id:int}")]
    public IResult RemoveBookingForTicket(int id)
    {
        _service.RemoveBooking(id);
        return Results.Ok();
    }

    [HttpPost]
    [Route("/theatreholic/api/tickets/buy/{id:int}")]
    public IResult BuyTicket(int id)
    {
        return _service.BuyTicket(id) ? Results.Ok() : Results.BadRequest();
    }

    [HttpPost]
    [Route("/theatreholic/api/tickets/book/{id:int}")]
    public IResult BookTicket(int id)
    {
        return _service.BookTicket(id) ? Results.Ok() : Results.BadRequest();
    }

    [HttpDelete]
    [Route("/theatreholic/api/tickets/{id:int}")]
    public IResult DeleteTicket(int id)
    {
        return _service.DeleteTicket(id) ? Results.Ok() : Results.BadRequest();
    }
}