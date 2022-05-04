using System.Text.Json.Serialization;

namespace TheatreHolic.WebApi.Dtos.Author;

public class Author
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    
    public Author(){}

    public Author(string name)
    {
        Name = name;
    }
}