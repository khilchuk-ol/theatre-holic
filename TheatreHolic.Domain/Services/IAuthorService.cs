using TheatreHolic.Domain.Models;

namespace TheatreHolic.Domain.Services;

public interface IAuthorService
{
    bool CreateAuthor(Author item);
    bool DeleteAuthor(int id);
    bool UpdateAuthor(Author item);
    IEnumerable<Author> FindAuthors(string? name, int offset, int amount);
    IEnumerable<Author> FindAuthorsByIds(List<int> ids, int offset, int amount);
}