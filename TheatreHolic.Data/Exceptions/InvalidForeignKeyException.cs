namespace TheatreHolic.Data.Exceptions;

public class InvalidForeignKeyException : Exception
{
    public string? Comment { get; }
    
    public InvalidForeignKeyException(string msg, string? comment = null) : base(msg)
    {
        Comment = comment;
    }
    
    public override string ToString()
    {
        return $"{nameof(InvalidForeignKeyException)} occured \nEXCEPTION: {Message} {(Comment != null ? $"[comment: {Comment}]" : "")}";
    }
}