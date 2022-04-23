using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreHolic.Data.Models;

public class Show
{
    public int Id { get; set; }

    public string Title { get; set; } = "";
    
    public int AuthorId { get; set; }
    [ForeignKey("AuthorId")]
    public Author? Author { get; set; }
    
    public List<Genre>? Genres { get; set; }
    
    public List<Ticket>? Tickets { get; set; }
    
    public DateTime Date { get; set; }
}