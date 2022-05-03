using TheatreHolic.Domain.Models;

namespace TheatreHolic.WebApi.Dtos;

public class Ticket
{
    public int Id { get; set; }
    
    public double Price { get; set; }
    
    public int Row { get; set; }
    
    public int Seat { get; set; }
    
    public TicketState State { get; set; }
    
    //public WebApi.Dtos.Show? Show { get; set; }
    public int ShowId { get; set; }
    
    public Ticket() {}

    public Ticket(double price, int row, int seat)
    {
        Price = price;
        Row = row;
        Seat = seat;
    }
}