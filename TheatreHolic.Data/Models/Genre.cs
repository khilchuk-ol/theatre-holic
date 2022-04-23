namespace TheatreHolic.Data.Models;

public class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    private List<Show>? _shows;
}