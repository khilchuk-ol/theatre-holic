namespace TheatreHolic.Domain.Services.Utils;

public class SearchShowsOptions
{
    public string? Title { get; set; }
    
    public DateTime? MinDateTime { get; set; }
    
    public DateTime? MaxDateTime { get; set; }

    public List<int>? AuthorIds { get; set; }
    
    public List<int>? GenreIds { get; set; }
}