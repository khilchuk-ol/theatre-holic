using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using TheatreHolic.Domain.Services.Utils;
using Xunit;

namespace TheatreHolic.Tests.Unit.ShowService;

public class FindShows
{
    [Fact]
    public void OptionsAreNull_GetAllData()
    {
        var showSvc = GetService();

        var shows = Fixtures.Shows;
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.GetPage(-1, -1)).Returns(shows);
        var res = showSvc.FindShows(null, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);

        Setup.ShowRepository.Setup(s => s.GetPage(2, 2)).Returns(shows.Skip(2).Take(2).ToList());
        res = showSvc.FindShows(null, 2, 2);

        res.Should().BeEquivalentTo(showsDomain.Skip(2).Take(2).ToList());
    }

    [Fact]
    public void FilterIsEmpty_GetAllData()
    {
        var showSvc = GetService();

        var shows = Fixtures.Shows;
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.GetPage(-1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions(), -1, -1);

        res.Should().BeEquivalentTo(showsDomain);

        Setup.ShowRepository.Setup(s => s.GetPage(2, 2)).Returns(shows.Skip(2).Take(2).ToList());
        res = showSvc.FindShows(new SearchShowsOptions(), 2, 2);

        res.Should().BeEquivalentTo(showsDomain.Skip(2).Take(2).ToList());
    }

    [Fact]
    public void TitleFilter_Success()
    {
        var showSvc = GetService();

        var title = "4";
        
        var p = Expression.Parameter(typeof(Data.Models.Show), "p");
        var body = Expression.Call(
            Expression.Property(p, typeof(Data.Models.Show), nameof(Data.Models.Show.Title)),
            typeof(string).GetMethod("Contains", new[] { typeof(string) }),
            Expression.Constant(title)
        );
        var expr = Expression.Lambda<Func<Data.Models.Show, bool>>(body, p);

        var shows = Fixtures.Shows.Where(s => s.Title.Contains(title)).ToList();
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.Filter(It.Is(
            (Expression<Func<Data.Models.Show, bool>> filter) => ExpressionEqualityComparer.Instance.Equals(filter, expr)), -1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions()
        {
            Title = title
        }, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
    }
    
    [Fact]
    public void TitleFilter_TitleIsWhiteSpace()
    {
        var showSvc = GetService();

        var title = "       ";
        
        var shows = Fixtures.Shows;
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.GetPage(-1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions()
        {
            Title = title
        }, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
    }
    
    [Fact]
    public void MinDateFilter_Success()
    {
        var showSvc = GetService();

        var minDate = DateTime.Now.AddDays(20);
        
        var p = Expression.Parameter(typeof(Data.Models.Show), "p");
        var body = Expression.GreaterThanOrEqual(
            Expression.Property(p, typeof(Data.Models.Show), nameof(Data.Models.Show.Date)),
            Expression.Constant(minDate)
        );
        var expr = Expression.Lambda<Func<Data.Models.Show, bool>>(body, p);

        var shows = Fixtures.Shows.Where(s => s.Date >= minDate).ToList();
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.Filter(It.Is(
            (Expression<Func<Data.Models.Show, bool>> filter) => ExpressionEqualityComparer.Instance.Equals(filter, expr)), -1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions()
        {
            MinDateTime = minDate
        }, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
    }
    
    [Fact]
    public void MinDateFilter_DateIsPast()
    {
        var showSvc = GetService();

        var minDate = DateTime.Now.AddDays(-20);
        
        var shows = Fixtures.Shows;
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.GetPage(-1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions()
        {
            MinDateTime = minDate
        }, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
    }
    
    [Fact]
    public void MaxDateFilter_Success()
    {
        var showSvc = GetService();

        var maxDate = DateTime.Now.AddDays(30);
        
        var p = Expression.Parameter(typeof(Data.Models.Show), "p");
        var body = Expression.LessThanOrEqual(
            Expression.Property(p, typeof(Data.Models.Show), nameof(Data.Models.Show.Date)),
            Expression.Constant(maxDate)
        );
        var expr = Expression.Lambda<Func<Data.Models.Show, bool>>(body, p);

        var shows = Fixtures.Shows.Where(s => s.Date <= maxDate).ToList();
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.Filter(It.Is(
            (Expression<Func<Data.Models.Show, bool>> filter) => ExpressionEqualityComparer.Instance.Equals(filter, expr)), -1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions()
        {
            MaxDateTime = maxDate
        }, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
    }
    
    [Fact]
    public void MaxDateFilter_DateIsPast()
    {
        var showSvc = GetService();

        var maxDate = DateTime.Now.AddDays(-20);
        
        var shows = Fixtures.Shows;
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.GetPage(-1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions()
        {
            MaxDateTime = maxDate
        }, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
    }
    
    [Fact]
    public void AuthorIdsFilter_Success()
    {
        var showSvc = GetService();

        var authorIds = new List<int> { 3, 4 };
        
        var p = Expression.Parameter(typeof(Data.Models.Show), "p");
        var body = Expression.Call(
            Expression.Constant(authorIds),
            typeof(List<int>).GetMethod("Contains", new[] { typeof(int) }),
            Expression.Property(p, typeof(Data.Models.Show), nameof(Data.Models.Show.AuthorId))
        );
        var expr = Expression.Lambda<Func<Data.Models.Show, bool>>(body, p);

        var shows = Fixtures.Shows.Where(s => authorIds.Contains(s.AuthorId)).ToList();
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.Filter(It.Is(
            (Expression<Func<Data.Models.Show, bool>> filter) => ExpressionEqualityComparer.Instance.Equals(filter, expr)), -1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions()
        {
            AuthorIds = authorIds
        }, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
    }
    
    [Fact]
    public void AuthorIdsFilter_Empty()
    {
        var showSvc = GetService();

        var authorIds = new List<int>();
        
        var shows = Fixtures.Shows;
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.GetPage(-1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions()
        {
            AuthorIds = authorIds
        }, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
    }
        
    [Fact]
    public void GenreIdsFilter_Success()
    {
        var showSvc = GetService();

        var genreIds = new List<int> { 3, 4 };
        
        var p = Expression.Parameter(typeof(Data.Models.Show), "p");
        var body = Expression.Call(
            Expression.Constant(genreIds),
            typeof(List<int>).GetMethod("Contains", new[] { typeof(int) }),
            Expression.Property(p, typeof(Data.Models.Show), nameof(Data.Models.Show.GenreId))
        );
        var expr = Expression.Lambda<Func<Data.Models.Show, bool>>(body, p);

        var shows = Fixtures.Shows.Where(s => genreIds.Contains(s.GenreId)).ToList();
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.Filter(It.Is(
            (Expression<Func<Data.Models.Show, bool>> filter) => ExpressionEqualityComparer.Instance.Equals(filter, expr)), -1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions()
        {
            GenreIds = genreIds
        }, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
    }
    
    [Fact]
    public void GenreIdsFilter_Empty()
    {
        var showSvc = GetService();

        var genreIds = new List<int>();
        
        var shows = Fixtures.Shows;
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.GetPage(-1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions()
        {
            GenreIds = genreIds
        }, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
    }

    private IShowService GetService()
    {
        return new Domain.Services.Impl.ShowService(Setup.ShowRepository.Object, Setup.Mapper,
            new Mock<ILogger<Domain.Services.Impl.ShowService>>().Object);
    }
}