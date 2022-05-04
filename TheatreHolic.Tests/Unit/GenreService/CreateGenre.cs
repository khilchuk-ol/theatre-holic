using Moq;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.GenreService;

public class CreateGenre
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var genreSvc = GetService();
        var genre = new Data.Models.Genre
        {
            Name = "new genre"
        };

        Setup.GenreRepository.Setup(s => s.Create(It.Is(
            (Data.Models.Genre d) => d.Equals(genre))));
        Assert.True(genreSvc.CreateGenre(Setup.Mapper.Map<TheatreHolic.Data.Models.Genre, Genre>(genre)));
    }

    [Fact]
    public void NameIsWhitespace_ReturnFalse()
    {
        var genreSvc = GetService();
        var genre = new Data.Models.Genre
        {
            Name = "        "
        };

        Assert.False(genreSvc.CreateGenre(Setup.Mapper.Map<TheatreHolic.Data.Models.Genre, Genre>(genre)));
    }
    
    private IGenreService GetService()
    {
        return new Domain.Services.Impl.GenreService(Setup.GenreRepository.Object, Setup.Mapper);
    }
}