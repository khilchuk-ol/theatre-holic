using Microsoft.Extensions.Logging;
using Moq;
using TheatreHolic.Data.Exceptions;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.TicketService;

public class CreateTicket
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var ticketSvc = GetService();
        var ticket = new Data.Models.Ticket()
        {
            Seat = 5,
            Row = 5,
            Price = 20,
            ShowId = 4,
            State = 0
        };

        Setup.TicketRepository.Setup(s =>
            s.Create(It.Is((Data.Models.Ticket t) => t.State == Data.Models.TicketState.Available)));
        Assert.True(ticketSvc.CreateTicket(Setup.Mapper.Map<TheatreHolic.Data.Models.Ticket, Ticket>(ticket)));
    }

    [Fact]
    public void PriceInvalid_ReturnFalse()
    {
        var ticketSvc = GetService();
        var ticket = new Data.Models.Ticket
        {
            Price = -10
        };

        Assert.False(ticketSvc.CreateTicket(Setup.Mapper.Map<TheatreHolic.Data.Models.Ticket, Ticket>(ticket)));
    }

    [Fact]
    public void RowInvalid_ReturnFalse()
    {
        var ticketSvc = GetService();
        var ticket = new Data.Models.Ticket
        {
            Row = -10
        };

        Assert.False(ticketSvc.CreateTicket(Setup.Mapper.Map<TheatreHolic.Data.Models.Ticket, Ticket>(ticket)));
    }

    [Fact]
    public void SeatInvalid_ReturnFalse()
    {
        var ticketSvc = GetService();
        var ticket = new Data.Models.Ticket
        {
            Seat = -10
        };

        Assert.False(ticketSvc.CreateTicket(Setup.Mapper.Map<TheatreHolic.Data.Models.Ticket, Ticket>(ticket)));
    }

    [Fact]
    public void RepositoryThrowException_ReturnFalse()
    {
        var ticketSvc = GetService();
        var ticket = new Ticket
        {
            Seat = 5,
            Row = 5,
            Price = 20,
            ShowId = 4,
            State = TicketState.Available
        };

        var model = Setup.Mapper.Map<Ticket, Data.Models.Ticket>(ticket);
        Setup.TicketRepository.Setup(s => s.Create(It.Is(
                (Data.Models.Ticket d) => d.Equals(model))))
            .Throws(new InvalidForeignKeyException("msg"));

        ticket.State = 0;

        Assert.False(ticketSvc.CreateTicket(ticket));
    }

    private ITicketService GetService()
    {
        return new Domain.Services.Impl.TicketService(Setup.TicketRepository.Object, Setup.Mapper,
            new Mock<ILogger<Domain.Services.Impl.TicketService>>().Object);
    }
}