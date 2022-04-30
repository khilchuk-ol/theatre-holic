using System.Linq;
using FluentAssertions;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.TicketService;

public class GetAvailableTickets
{
    [Fact]
    public void TicketExists_Success()
    {
        var ticketSvc = GetService();
        
        var showId = 2;

        var tickets = Fixtures.Tickets.Where(t => t.ShowId == showId && t.State == Data.Models.TicketState.Available).ToList();
        var ticketsDomain = tickets.Select(t => Setup.Mapper.Map<TheatreHolic.Data.Models.Ticket, Ticket>(t)).ToList();
        
        Setup.TicketRepository.Setup(repo => repo.Filter(t => t.State == Data.Models.TicketState.Available && t.ShowId == showId, -1, -1)).Returns(tickets);

        var res = ticketSvc.GetAvailableTickets(showId);
        
        res.Should().BeEquivalentTo(ticketsDomain);
    }
    
    private ITicketService GetService()
    {
        return new Domain.Services.Impl.TicketService(Setup.TicketRepository.Object, Setup.Mapper);
    }
}