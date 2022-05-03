using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheatreHolic.WebApi.Dtos.Show;

public class UpdateShowDto
{
    [Required]
    public int Id { get; set; }
    
    [RegularExpression("^[a-zA-Z -.0-9]+")]
    [StringLength(50, ErrorMessage = "Title length can't be more than 50 nad less than 3.", MinimumLength = 3)]
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("author_id")]
    public int? AuthorId { get; set; }
    
    [JsonPropertyName("genre_id")]
    public int? GenreId { get; set; }
        
    [JsonPropertyName("dt")]
    public DateTime? Date { get; set; }
}