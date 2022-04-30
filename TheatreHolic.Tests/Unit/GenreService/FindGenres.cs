using System.Linq;
using FluentAssertions;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.GenreService;

public class FindGenres
{
    [Fact]
    public void NameIsNull_GetAllData()
    {
        var genreSvc = GetService();
        
        var genres = Fixtures.Genres;
        var genresDomain = genres.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Genre, Genre>(s)).ToList();

        Setup.GenreRepository.Setup(repo => repo.GetPage(-1, -1)).Returns(genres);
        var res = genreSvc.FindGenres(null, -1, -1);

        res.Should().BeEquivalentTo(genresDomain);

        Setup.GenreRepository.Setup(repo => repo.GetPage(2, 2)).Returns(genres.Skip(2).Take(2).ToList());
        res = genreSvc.FindGenres(null, 2, 2);

        res.Should().BeEquivalentTo(genresDomain.Skip(2).Take(2).ToList());
    }
    
    [Fact]
    public void NameIsWhitespace_GetAllData()
    {
        var genreSvc = GetService();
        var name = "    ";
        
        var genres = Fixtures.Genres;
        var genresDomain = genres.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Genre, Genre>(s)).ToList();

        Setup.GenreRepository.Setup(repo => repo.GetPage(-1, -1)).Returns(genres);
        var res = genreSvc.FindGenres(name, -1, -1);

        res.Should().BeEquivalentTo(genresDomain);

        Setup.GenreRepository.Setup(repo => repo.GetPage(2, 2)).Returns(genres.Skip(2).Take(2).ToList());
        res = genreSvc.FindGenres(name, 2, 2);

        res.Should().BeEquivalentTo(genresDomain.Skip(2).Take(2).ToList());
    }
    
    [Fact]
    public void Success()
    {
        var genreSvc = GetService();
        var name = "3";
        
        var genres = Fixtures.Genres.Where(g => g.Name.Contains(name)).ToList();
        var genresDomain = genres.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Genre, Genre>(s)).ToList();

        Setup.GenreRepository.Setup(repo => repo.Filter(g => g.Name.Contains(name), -1, -1)).Returns(genres);
        var res = genreSvc.FindGenres(name, -1, -1);

        res.Should().BeEquivalentTo(genresDomain);

        Setup.GenreRepository.Setup(repo => repo.Filter(g => g.Name.Contains(name), 2, 2)).Returns(genres.Skip(2).Take(2).ToList());
        res = genreSvc.FindGenres(name, 2, 2);

        res.Should().BeEquivalentTo(genresDomain.Skip(2).Take(2).ToList());
    }

    private IGenreService GetService()
    {
        return new Domain.Services.Impl.GenreService(Setup.GenreRepository.Object, Setup.Mapper);
    }
}