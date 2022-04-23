using TheatreHolic.Data.Models;

namespace TheatreHolic.Data.Repository;

public interface IShowRepository: IRepository<int, Show>
{
    IEnumerable<Show> FindAllWithData();
    Show? FindWithData(int id);
    IEnumerable<Show> FilterWithData(Func<Show, bool> filter, int offset, int amount);
    IEnumerable<Show> GetPageWithData(int offset, int amount);
}