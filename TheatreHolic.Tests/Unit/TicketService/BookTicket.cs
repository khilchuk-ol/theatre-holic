using Microsoft.Extensions.Logging;
using Moq;
using TheatreHolic.Data.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.TicketService;

public class BookTicket
{
    [Fact]
    public void TicketDoesNotExist_ReturnFalse()
    {
        var ticketSvc = GetService();
        var id = 10;

        Setup.TicketRepository.Setup(repo => repo.Find(id)).Returns((Ticket?)null);
        Assert.False(ticketSvc.BookTicket(id));
    }
    
    [Theory]
    [InlineData(TicketState.Booked)]
    [InlineData(TicketState.Sold)]
    public void TicketStateIsNotValid_ReturnFalse(TicketState state)
    {
        var ticketSvc = GetService();
        var ticket = new Ticket()
        {
            Id = 10,
            State = state
        };

        Setup.TicketRepository.Setup(repo => repo.Find(ticket.Id)).Returns(ticket);
        Assert.False(ticketSvc.BookTicket(ticket.Id));
    }
    
    [Fact]
    public void TicketExists_Success()
    {
        var ticketSvc = GetService();
        var ticket = new Ticket()
        {
            Id = 10,
            State = TicketState.Available
        };

        Setup.TicketRepository.Setup(repo => repo.Find(ticket.Id)).Returns(ticket);
        Setup.TicketRepository.Setup(repo => repo.Update(It.Is((Ticket t) => t.State == TicketState.Booked)));
        
        Assert.True(ticketSvc.BookTicket(ticket.Id));
    }
    
    private ITicketService GetService()
    {
        return new Domain.Services.Impl.TicketService(Setup.TicketRepository.Object, Setup.Mapper,
            new Mock<ILogger<Domain.Services.Impl.TicketService>>().Object);
    }
}