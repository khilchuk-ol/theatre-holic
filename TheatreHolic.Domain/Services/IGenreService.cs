using TheatreHolic.Domain.Models;

namespace TheatreHolic.Domain.Services;

public interface IGenreService
{
    bool CreateGenre(Genre item);
    bool DeleteGenre(int id);
    bool UpdateGenre(Genre item);
    IEnumerable<Genre> FindGenres(string? name, int offset, int amount);
    IEnumerable<Genre> FindGenresByIds(List<int> ids, int offset, int amount);
}