using System.Linq.Expressions;
using AutoMapper;
using TheatreHolic.Data.Repository;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services.Utils;

namespace TheatreHolic.Domain.Services.Impl;

public class ShowService : IShowService
{
    private readonly IShowRepository _repository;
    private readonly IMapper _mapper;

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

        var filter = prepareFilter(opts);
        if (filter == null)
        {
            return _repository.GetPage(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s));
        }

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

        var filter = prepareFilter(opts);
        if (filter == null)
        {
            return _repository.GetPage(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s));
        }

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

    private Expression<Func<Data.Models.Show, bool>>? prepareFilter(SearchShowsOptions opts)
    {
        var p = Expression.Parameter(typeof(Data.Models.Show), "p");

        List<Expression> bodies = new List<Expression>();

        if (!String.IsNullOrEmpty(opts.Title?.Trim()))
        {
            opts.Title = opts.Title.Trim();

            var body = Expression.Call(
                Expression.Property(p, typeof(Data.Models.Show), nameof(Data.Models.Show.Title)),
                typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                Expression.Constant(opts.Title)
            );

            bodies.Add(body);
        }

        if (opts.MinDateTime != null && opts.MinDateTime >= DateTime.Now)
        {
            var body = Expression.GreaterThanOrEqual(
                Expression.Property(p, typeof(Data.Models.Show), nameof(Data.Models.Show.Date)),
                Expression.Constant(opts.MinDateTime)
            );

            bodies.Add(body);
        }
        
        if (opts.MaxDateTime != null && opts.MaxDateTime >= DateTime.Now)
        {
            var body = Expression.LessThanOrEqual(
                Expression.Property(p, typeof(Data.Models.Show), nameof(Data.Models.Show.Date)),
                Expression.Constant(opts.MaxDateTime)
            );

            bodies.Add(body);
        }
        
        if (opts.AuthorIds != null && opts.AuthorIds.Count > 0)
        {
            var body = Expression.Call(
                Expression.Constant(opts.AuthorIds),
                typeof(List<int>).GetMethod("Contains", new[] { typeof(int) }),
                Expression.Property(p, typeof(Data.Models.Show), nameof(Data.Models.Show.AuthorId))
            );

            bodies.Add(body);
        }
        
        if (opts.GenreIds != null && opts.GenreIds.Count > 0)
        {
            var body = Expression.Call(
                Expression.Constant(opts.GenreIds),
                typeof(List<int>).GetMethod("Contains", new[] { typeof(int) }),
                Expression.Property(p, typeof(Data.Models.Show), nameof(Data.Models.Show.GenreId))
            );

            bodies.Add(body);
        }

        if (!bodies.Any())
        {
            return null;
        }

        Expression aggregatedBody = bodies.Aggregate(Expression.AndAlso);

        return Expression.Lambda<Func<Data.Models.Show, bool>>(aggregatedBody, p);
    }
}