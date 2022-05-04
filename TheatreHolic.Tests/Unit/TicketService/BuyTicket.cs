using Microsoft.Extensions.Logging;
using Moq;
using TheatreHolic.Data.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.TicketService;

public class BuyTicket
{
    [Fact]
    public void TicketDoesNotExist_ReturnFalse()
    {
        var ticketSvc = GetService();
        var id = 10;

        Setup.TicketRepository.Setup(repo => repo.Find(id)).Returns((Ticket?)null);
        Assert.False(ticketSvc.BuyTicket(id));
    }
    
    [Fact]
    public void TicketStateIsNotValid_ReturnFalse()
    {
        var ticketSvc = GetService();
        var ticket = new Ticket()
        {
            Id = 10,
            State = TicketState.Sold
        };

        Setup.TicketRepository.Setup(repo => repo.Find(ticket.Id)).Returns(ticket);
        Assert.False(ticketSvc.BuyTicket(ticket.Id));
    }
    
    [Theory]
    [InlineData(TicketState.Booked)]
    [InlineData(TicketState.Available)]
    public void TicketExists_ValidState_Success(TicketState state)
    {
        var ticketSvc = GetService();
        var ticket = new Ticket()
        {
            Id = 10,
            State = state
        };

        Setup.TicketRepository.Setup(repo => repo.Find(ticket.Id)).Returns(ticket);
        Setup.TicketRepository.Setup(repo => repo.Update(It.Is((Ticket t) => t.State == TicketState.Sold)));
        
        Assert.True(ticketSvc.BuyTicket(ticket.Id));
    }
    
    private ITicketService GetService()
    {
        return new Domain.Services.Impl.TicketService(Setup.TicketRepository.Object, Setup.Mapper,
            new Mock<ILogger<Domain.Services.Impl.TicketService>>().Object);
    }
}