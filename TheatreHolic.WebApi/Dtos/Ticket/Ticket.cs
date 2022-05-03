using System.Text.Json.Serialization;

namespace TheatreHolic.WebApi.Dtos.Ticket;

public class Ticket
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("price")]
    public double Price { get; set; }
    
    [JsonPropertyName("row")]
    public int Row { get; set; }
    
    [JsonPropertyName("seat")]
    public int Seat { get; set; }
    
    [JsonPropertyName("state")]
    public TicketState State { get; set; }

    [JsonPropertyName("state_name")] 
    public string StateName => State.ToString();
    
    [JsonPropertyName("show_id")] 
    public int ShowId { get; set; }
    
    public Ticket() {}

    public Ticket(double price, int row, int seat)
    {
        Price = price;
        Row = row;
        Seat = seat;
    }
}