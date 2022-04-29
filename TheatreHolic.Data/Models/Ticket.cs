using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreHolic.Data.Models;

public class Ticket : IEquatable<Ticket>
{
    public int Id { get; set; }
    
    public double Price { get; set; }
    
    public int Row { get; set; }
    
    public int Seat { get; set; }
    
    public TicketState State { get; set; }
    
    [ForeignKey("ShowId")]
    public Show? Show { get; set; }
    public int ShowId { get; set; }
    public bool Equals(Ticket? other)
    {
        if (other == null)
        {
            return false;
        }

        return Math.Abs(Price - other.Price) < 0.01 &&
               Row == other.Row &&
               Seat == other.Seat &&
               ShowId == other.ShowId &&
               State == other.State;
    }
}