using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheatreHolic.WebApi.Dtos.Ticket;

public class CreateTicketDto
{
    [Required]
    [Range(0, 1000, ErrorMessage = "Price should be in 0-1000 range")]
    [JsonPropertyName("price")]
    public double Price { get; set; }
  
    [Required]
    [Range(0, 100, ErrorMessage = "Row should be in 0-100 range")]
    [JsonPropertyName("row")]
    public int Row { get; set; }
   
    [Required]
    [Range(0, 100, ErrorMessage = "Seat should be in 0-100 range")]
    [JsonPropertyName("seat")]
    public int Seat { get; set; }
    
    [Required]
    [JsonPropertyName("show_id")] 
    public int ShowId { get; set; }
}