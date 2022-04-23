namespace TheatreHolic.Domain.Models;

public class Show
{
    public int Id { get; set; }

    public string Title { get; set; } = "";
    
    public Author? Author { get; set; }
    
    public List<Genre>? Genres { get; set; }
    
    public List<Ticket>? Tickets { get; set; }
    
    public DateTime Date { get; set; }
    
    public Show() {}

    public Show(string title, DateTime dt)
    {
        Title = title;
        Date = dt;
    }
}