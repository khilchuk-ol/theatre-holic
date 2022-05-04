using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.GenreService;

public class UpdateGenre
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var genreSvc = GetService();
        var genre = new Data.Models.Genre();
        genre.Id = 10;
        genre.Name = "name";

        Setup.GenreRepository.Setup(s => s.Find(genre.Id)).Returns(genre);
        Setup.GenreRepository.Setup(s => s.Update(genre));
        
        Assert.True(genreSvc.UpdateGenre(Setup.Mapper.Map<TheatreHolic.Data.Models.Genre, Genre>(genre)));
    }
    
    [Fact]
    public void NoSuchItem_ReturnFalse()
    {
        var genreSvc = GetService();
        var genre = new Data.Models.Genre();
        genre.Id = 10;
        genre.Name = "name";
        
        Setup.GenreRepository.Setup(s => s.Find(genre.Id)).Returns((Data.Models.Genre?)null);
        
        Assert.False(genreSvc.UpdateGenre(Setup.Mapper.Map<TheatreHolic.Data.Models.Genre, Genre>(genre)));
    }
    
    [Fact]
    public void TitleIsWhitespace_ReturnFalse()
    {
        var genreSvc = GetService();
        var genre = new Data.Models.Genre();
        genre.Id = 10;
        genre.Name = "     ";
        
        Setup.GenreRepository.Setup(s => s.Find(genre.Id)).Returns(genre);
        
        Assert.False(genreSvc.UpdateGenre(Setup.Mapper.Map<TheatreHolic.Data.Models.Genre, Genre>(genre)));
    }
    
    private IGenreService GetService()
    {
        return new Domain.Services.Impl.GenreService(Setup.GenreRepository.Object, Setup.Mapper);
    }
}