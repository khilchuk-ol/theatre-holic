using System.Linq.Expressions;
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
            return _repository.GetPage(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s));
        }

        var filter = prepareFilterExpr(opts);

        return _repository.Filter(filter, offset, amount)
            .Select(s => _mapper.Map<Data.Models.Show, Show>(s));
    }

    public IEnumerable<Show> FindShowsWithInfo(SearchShowsOptions? opts, int offset, int amount)
    {
        if (opts == null)
        {
            return _repository.GetPageWithData(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s));
        }

        var filter = prepareFilterExpr(opts);

        return _repository.FilterWithData(filter, offset, amount)
            .Select(s => _mapper.Map<Data.Models.Show, Show>(s));
    }

    public IEnumerable<Show> GetShowsByIds(List<int> ids, int offset, int amount)
    {
        if (!ids.Any())
        {
            return _repository.GetPage(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s));
        }

        return _repository.Filter(
            s => ids.Contains(s.Id),
            offset,
            amount)
            .Select(s => _mapper.Map<Data.Models.Show, Show>(s));
    }

    public IEnumerable<Show> GetShowsByIdsWithInfo(List<int> ids, int offset, int amount)
    {
        if (!ids.Any())
        {
            return _repository.GetPageWithData(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s));
        }

        return _repository.FilterWithData(
            s => ids.Contains(s.Id),
            offset,
            amount)
            .Select(s => _mapper.Map<Data.Models.Show, Show>(s));
    }
    
    private Expression<Func<Data.Models.Show, bool>> prepareFilterExpr(SearchShowsOptions opts)
    {
        Expression<Func<Data.Models.Show, bool>> filter = s => true;
        Expression<Func<Data.Models.Show, bool>> temp;

        if (!String.IsNullOrEmpty(opts.Title?.Trim()))
        {
            temp = s => s.Title.Contains(opts.Title.Trim());
            var body = Expression.AndAlso(filter.Body, temp.Body);
            
            filter = Expression.Lambda<Func<Data.Models.Show,bool>>(body, filter.Parameters[0]);
        }
        
        if (opts.MinDateTime != null && opts.MinDateTime >= DateTime.Now)
        {
            temp = s => s.Date >= opts.MinDateTime;
            var body = Expression.AndAlso(filter.Body, temp.Body);
            
            filter = Expression.Lambda<Func<Data.Models.Show,bool>>(body, filter.Parameters[0]);
        }
        
        if (opts.MaxDateTime != null && opts.MaxDateTime >= DateTime.Now)
        {
            temp = s => s.Date <= opts.MaxDateTime;
            var body = Expression.AndAlso(filter.Body, temp.Body);
            
            filter = Expression.Lambda<Func<Data.Models.Show,bool>>(body, filter.Parameters[0]);
        }
        
        if (opts.AuthorIds != null && opts.AuthorIds.Count > 0)
        {
            temp = s => opts.AuthorIds.Contains(s.AuthorId);
            var body = Expression.AndAlso(filter.Body, temp.Body);
            
            filter = Expression.Lambda<Func<Data.Models.Show,bool>>(body, filter.Parameters[0]);
        }
        
        if (opts.GenreIds != null && opts.GenreIds.Count > 0)
        {
            temp = s => s.Genres != null && s.Genres.Select(g => g.Id).Intersect(opts.GenreIds).Any();
            var body = Expression.AndAlso(filter.Body, temp.Body);
            
            filter = Expression.Lambda<Func<Data.Models.Show,bool>>(body, filter.Parameters[0]);
        }
        
        return filter;
    }
}