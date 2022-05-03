using Microsoft.AspNetCore.Mvc;

namespace TheatreHolic.WebApi.Dtos.Show;

public class SearchFilter
{
    [BindProperty(Name = "title")] 
    public string? Title { get; set; }

    [BindProperty(Name = "min_dt")] 
    public DateTime? MinDateTime { get; set; }

    [BindProperty(Name = "max_dt")] 
    public DateTime? MaxDateTime { get; set; }

    [BindProperty(Name = "author_ids")]
    public List<int> AuthorIds { get; set; } = new();

    [BindProperty(Name = "genre_ids")] 
    public List<int> GenreIds { get; set; } = new();
}