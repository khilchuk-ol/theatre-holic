using TheatreHolic.Domain.Models;

namespace TheatreHolic.Domain.Services;

public interface IGenreService
{
    IEnumerable<Genre> FindGenres(string? name, int offset, int amount);
    IEnumerable<Genre> FindGenresByIds(List<int> ids, int offset, int amount);
}