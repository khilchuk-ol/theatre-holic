namespace TheatreHolic.Data.Models;

public class Genre : IEquatable<Genre>
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public bool Equals(Genre? other)
    {
        if (other == null)
        {
            return false;
        }

        return Name == other.Name;
    }
}