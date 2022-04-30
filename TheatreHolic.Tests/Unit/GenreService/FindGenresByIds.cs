using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.GenreService;

public class FindGenresByIds
{
    [Fact]
    public void IdsAreEmpty_GetAllData()
    {
        var genreSvc = GetService();

        var genres = Fixtures.Genres;
        var genresDomain = genres.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Genre, Genre>(s)).ToList();

        Setup.GenreRepository.Setup(repo => repo.GetPage(-1, -1)).Returns(genres);
        var res = genreSvc.FindGenresByIds(new List<int>(), -1, -1);

        res.Should().BeEquivalentTo(genresDomain);

        Setup.GenreRepository.Setup(repo => repo.GetPage(2, 2)).Returns(genres.Skip(2).Take(2).ToList());
        res = genreSvc.FindGenresByIds(new List<int>(), 2, 2);

        res.Should().BeEquivalentTo(genresDomain.Skip(2).Take(2).ToList());
    }
    
    [Fact]
    public void Success()
    {
        var genreSvc = GetService();
        var ids = new List<int> { 2, 3, 4, 5};

        var genres = Fixtures.Genres.Where(g => ids.Contains(g.Id)).ToList();
        var genresDomain = genres.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Genre, Genre>(s)).ToList();
        
        Setup.GenreRepository.Setup(repo => repo.Filter(g => ids.Contains(g.Id), -1, -1)).Returns(genres);
        var res = genreSvc.FindGenresByIds(ids, -1, -1);

        res.Should().BeEquivalentTo(genresDomain);
        
        Setup.GenreRepository.Setup(repo => repo.Filter(g => ids.Contains(g.Id), 2, 1)).Returns(genres.Skip(2).Take(1).ToList());
        res = genreSvc.FindGenresByIds(ids, 2, 1);

        res.Should().BeEquivalentTo(genresDomain.Skip(2).Take(1).ToList());
    }
    
    private IGenreService GetService()
    {
        return new Domain.Services.Impl.GenreService(Setup.GenreRepository.Object, Setup.Mapper);
    }
}