using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreHolic.Data.Models;

public class Ticket
{
    public int Id { get; set; }
    
    public double Price { get; set; }
    
    public int Row { get; set; }
    
    public int Seat { get; set; }
    
    public TicketState State { get; set; }
    
    [ForeignKey("ShowId")]
    public Show? Show { get; set; }
    public int ShowId { get; set; }
}