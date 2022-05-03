using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatreHolic.Domain.Services;

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
}