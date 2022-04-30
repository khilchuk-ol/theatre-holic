using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.TicketService;

public class CreateTicket
{
    [Fact]
    public void Success()
    {
        var ticketSvc = GetService();
        var ticket = Fixtures.Tickets[0];

        Setup.TicketRepository.Setup(s => s.Create(ticket));
        ticketSvc.CreateTicket(Setup.Mapper.Map<TheatreHolic.Data.Models.Ticket, Ticket>(ticket));
    }
    
    private ITicketService GetService()
    {
        return new Domain.Services.Impl.TicketService(Setup.TicketRepository.Object, Setup.Mapper);
    }
}