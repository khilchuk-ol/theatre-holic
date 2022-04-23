namespace TheatreHolic.Domain.Models;

public class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
    
    public Author(){}

    public Author(string name)
    {
        Name = name;
    }
}