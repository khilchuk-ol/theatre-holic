using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheatreHolic.WebApi.Dtos;

public class Author
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [Required]
    [RegularExpression("^[a-zA-Z -.0-9]+")]
    [StringLength(25, ErrorMessage = "Name length can't be more than 25 nad less than 3.", MinimumLength = 3)]
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    
    public Author(){}

    public Author(string name)
    {
        Name = name;
    }
}