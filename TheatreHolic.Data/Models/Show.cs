using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreHolic.Data.Models;

public class Show : IEquatable<Show>
{
    public int Id { get; set; }

    public string Title { get; set; } = "";
    
    public int AuthorId { get; set; }
    [ForeignKey("AuthorId")]
    public Author? Author { get; set; }
    
    public int GenreId { get; set; }
    [ForeignKey("GenreId")]
    public Genre? Genre { get; set; }
    
    public List<Ticket>? Tickets { get; set; }
    
    public DateTime Date { get; set; }
    
    public bool Equals(Show? other)
    {
        if (other == null)
        {
            return false;
        }

        return Title == other.Title &&
               Date == other.Date &&
               GenreId == other.GenreId &&
               AuthorId == other.AuthorId;
    }
}