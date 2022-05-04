using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.GenreService;

public class DeleteGenre
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var genreSvc = GetService();
        var genre = Fixtures.Genres[0];

        Setup.GenreRepository.Setup(s => s.Remove(genre.Id)).Returns(true);
        Assert.True(genreSvc.DeleteGenre(genre.Id));
    }
    
    [Fact]
    public void RepositoryReturnFalse_ReturnFalse()
    {
        var genreSvc = GetService();
        var genre = Fixtures.Genres[0];

        Setup.GenreRepository.Setup(s => s.Remove(genre.Id)).Returns(false);
        Assert.False(genreSvc.DeleteGenre(genre.Id));
    }
    
    private IGenreService GetService()
    {
        return new Domain.Services.Impl.GenreService(Setup.GenreRepository.Object, Setup.Mapper);
    }
}