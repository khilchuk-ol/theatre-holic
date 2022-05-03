using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TheatreHolic.Data.Exceptions;
using TheatreHolic.Data.Repository;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services.Utils;

namespace TheatreHolic.Domain.Services.Impl;

public class ShowService : IShowService
{
    private readonly IShowRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ShowService> _logger;

    public ShowService(IShowRepository repository, IMapper mapper, ILogger<ShowService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public bool CreateShow(Show item)
    {
        if (item.Date <= DateTime.Now)
        {
            return false;
        }
        
        try
        {
            var model = _mapper.Map<Show, Data.Models.Show>(item);
            _repository.Create(model);
        }
        catch (InvalidForeignKeyException e)
        {
            _logger.Log(LogLevel.Warning, e.ToString());
            return false;
        }

        return true;
    }

    public bool DeleteShow(int id)
    {
        return _repository.Remove(id);
    }

    public bool UpdateShow(Show item)
    {
        var toPatch = _repository.Find(item.Id);
        if (toPatch == null) return false;

        if (item.Title != null)
        {
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                return false;
            }
            
            toPatch.Title = item.Title.Trim();
        }

        if (item.Date != null)
        {
            if (item.Date <= DateTime.Now)
            {
                return false;
            }
            toPatch.Date = item.Date.Value;
        }
        
        if (item.Author != null)
        {
            toPatch.AuthorId = item.Author.Id;
        }
        
        if (item.Genre != null)
        {
            toPatch.GenreId = item.Genre.Id;
        }
        
        try
        {
            _repository.Update(toPatch);
        }
        catch (InvalidForeignKeyException e)
        {
            _logger.Log(LogLevel.Warning, e.ToString());
            return false;
        }

        return true;
    }

    public IEnumerable<Show> FindShows(SearchShowsOptions? opts, int offset = 0, int amount = 0)
    {
        if (opts == null)
        {
            return _repository.GetPage(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s))
                .ToList();
        }

        var filter = prepareFilter(opts);
        if (filter == null)
        {
            return _repository.GetPage(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s))
                .ToList();
        }

        return _repository.Filter(filter, offset, amount)
            .Select(s => _mapper.Map<Data.Models.Show, Show>(s))
            .ToList();
    }

    public IEnumerable<Show> FindShowsWithInfo(SearchShowsOptions? opts, int offset, int amount)
    {
        if (opts == null)
        {
            return _repository.GetPageWithData(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s))
                .ToList();
        }

        var filter = prepareFilter(opts);
        if (filter == null)
        {
            return _repository.GetPageWithData(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s))
                .ToList();
        }

        return _repository.FilterWithData(filter, offset, amount)
            .Select(s => _mapper.Map<Data.Models.Show, Show>(s))
            .ToList();
    }

    public IEnumerable<Show> GetShowsByIds(List<int> ids, int offset, int amount)
    {
        if (!ids.Any())
        {
            return _repository.GetPage(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s))
                .ToList();
        }

        return _repository.Filter(
                s => ids.Contains(s.Id),
                offset,
                amount)
            .Select(s => _mapper.Map<Data.Models.Show, Show>(s))
            .ToList();
    }

    public IEnumerable<Show> GetShowsByIdsWithInfo(List<int> ids, int offset, int amount)
    {
        if (!ids.Any())
        {
            return _repository.GetPageWithData(offset, amount)
                .Select(s => _mapper.Map<Data.Models.Show, Show>(s))
                .ToList();
        }

        return _repository.FilterWithData(
                s => ids.Contains(s.Id),
                offset,
                amount)
            .Select(s => _mapper.Map<Data.Models.Show, Show>(s))
            .ToList();
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