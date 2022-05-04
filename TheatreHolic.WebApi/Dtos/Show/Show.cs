using System.Text.Json.Serialization;

namespace TheatreHolic.WebApi.Dtos.Show;

public class Show
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";
    
    [JsonPropertyName("author")]
    public Author.Author? Author { get; set; }

    [JsonPropertyName("genre")]
    public Genre.Genre? Genre { get; set; }

    [JsonPropertyName("dt")]
    public DateTime Date { get; set; }
    
    public Show() {}

    public Show(string title, DateTime dt)
    {
        Title = title;
        Date = dt;
    }
}