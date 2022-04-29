namespace TheatreHolic.Data.Models;

public class Author : IEquatable<Author>
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public bool Equals(Author? other)
    {
        if (other == null)
        {
            return false;
        }

        return Name == other.Name;
    }
}