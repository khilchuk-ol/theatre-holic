using Microsoft.Extensions.Logging;
using Moq;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.TicketService;

public class DeleteTicket
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var ticketSvc = GetService();
        var ticket = Fixtures.Tickets[0];

        Setup.TicketRepository.Setup(s => s.Remove(ticket.Id)).Returns(true);
        Assert.True(ticketSvc.DeleteTicket(ticket.Id));
    }
    
    [Fact]
    public void RepositoryReturnFalse_ReturnFalse()
    {
        var ticketSvc = GetService();
        var ticket = Fixtures.Tickets[0];

        Setup.TicketRepository.Setup(s => s.Remove(ticket.Id)).Returns(false);
        Assert.False(ticketSvc.DeleteTicket(ticket.Id));
    }
    
    private ITicketService GetService()
    {
        return new Domain.Services.Impl.TicketService(Setup.TicketRepository.Object, Setup.Mapper,
            new Mock<ILogger<Domain.Services.Impl.TicketService>>().Object);
    }
}