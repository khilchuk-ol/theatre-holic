using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services.Utils;

namespace TheatreHolic.Domain.Services;

public interface IShowService
{
    bool CreateShow(Show item);
    bool DeleteShow(int id);
    bool UpdateShow(Show item);
    
    IEnumerable<Show> FindShows(SearchShowsOptions? opts, int offset, int amount);
    IEnumerable<Show> FindShowsWithInfo(SearchShowsOptions? opts, int offset, int amount);

    IEnumerable<Show> GetShowsByIds(List<int> ids, int offset, int amount);
    IEnumerable<Show> GetShowsByIdsWithInfo(List<int> ids, int offset, int amount);
}