using AutoMapper;
using TheatreHolic.Data.Repository;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services.Utils;

namespace TheatreHolic.Domain.Services.Impl;

public class ShowService : IShowService
{
    private IShowRepository _repository;
    private IMapper _mapper;

    public ShowService(IShowRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public void CreateShow(Show item)
    {
        _repository.Create(_mapper.Map<Show, Data.Models.Show>(item));
    }

    public void DeleteShow(int id)
    {
        _repository.Remove(id);
    }

    public void UpdateShow(Show item)
    {
        _repository.Update(_mapper.Map<Show, Data.Models.Show>(item));
    }

    public IEnumerable<Show> FindShows(SearchShowsOptions? opts, int offset = 0, int amount = 0)
    {
        if (opts == null)
        {
            return _repository.GetPage(offset, amount).Select(s => _mapper.Map<Data.Models.Show, Show>(s));
        }

        var filter = prepareFilter(opts);

        return _repository.Filter(filter, offset, amount).Select(s => _mapper.Map<Data.Models.Show, Show>(s));
    }

    public IEnumerable<Show> FindShowsWithInfo(SearchShowsOptions? opts, int offset, int amount)
    {
        if (opts == null)
        {
            return _repository.GetPageWithData(offset, amount).Select(s => _mapper.Map<Data.Models.Show, Show>(s));
        }

        var filter = prepareFilter(opts);

        return _repository.FilterWithData(filter, offset, amount).Select(s => _mapper.Map<Data.Models.Show, Show>(s));
    }

    public IEnumerable<Show> GetShowsByIds(List<int> ids, int offset, int amount)
    {
        if (!ids.Any())
        {
            return _repository.GetPage(offset, amount).Select(s => _mapper.Map<Data.Models.Show, Show>(s));
        }

        return _repository.Filter(
            s => ids.Contains(s.Id),
            offset,
            amount).Select(s => _mapper.Map<Data.Models.Show, Show>(s));
    }

    public IEnumerable<Show> GetShowsByIdsWithInfo(List<int> ids, int offset, int amount)
    {
        if (!ids.Any())
        {
            return _repository.GetPageWithData(offset, amount).Select(s => _mapper.Map<Data.Models.Show, Show>(s));
        }

        return _repository.FilterWithData(
            s => ids.Contains(s.Id),
            offset,
            amount).Select(s => _mapper.Map<Data.Models.Show, Show>(s));
    }

    private Func<Data.Models.Show, bool> prepareFilter(SearchShowsOptions opts)
    {
        Func<Data.Models.Show, bool>? filter = null;

        if (!String.IsNullOrEmpty(opts.Title?.Trim()))
        {
            filter += show => show.Title.Contains(opts.Title.Trim());
        }
        
        if (opts.MinDateTime != null && opts.MinDateTime >= DateTime.Now)
        {
            filter += show => show.Date >= opts.MinDateTime;
        }
        
        if (opts.MaxDateTime != null && opts.MaxDateTime >= DateTime.Now)
        {
            filter += show => show.Date <= opts.MaxDateTime;
        }
        
        if (opts.AuthorIds != null && opts.AuthorIds.Count > 0)
        {
            filter += show => opts.AuthorIds.Contains(show.AuthorId);
        }
        
        if (opts.GenreIds != null && opts.GenreIds.Count > 0)
        {
            filter += show => show.Genres.Select(g => g.Id).Intersect(opts.GenreIds).Any();
        }

        return filter;
    }
}