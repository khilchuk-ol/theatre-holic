namespace TheatreHolic.Domain.Services.Utils;

public class SearchShowsOptions
{
    public string? Title { get; set; } = null;

    public DateTime? MinDateTime { get; set; } = null;

    public DateTime? MaxDateTime { get; set; } = null;

    public List<int>? AuthorIds { get; set; } = null;

    public List<int>? GenreIds { get; set; } = null;
}