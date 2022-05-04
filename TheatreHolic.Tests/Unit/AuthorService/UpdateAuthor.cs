using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.AuthorService;

public class UpdateAuthor
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var authorSvc = GetService();
        var author = new Data.Models.Author();
        author.Id = 10;
        author.Name = "name";

        Setup.AuthorRepository.Setup(s => s.Find(author.Id)).Returns(author);
        Setup.AuthorRepository.Setup(s => s.Update(author));
        
        Assert.True(authorSvc.UpdateAuthor(Setup.Mapper.Map<TheatreHolic.Data.Models.Author, Author>(author)));
    }
    
    [Fact]
    public void NoSuchItem_ReturnFalse()
    {
        var authorSvc = GetService();
        var author = new Data.Models.Author();
        author.Id = 10;
        author.Name = "name";
        
        Setup.AuthorRepository.Setup(s => s.Find(author.Id)).Returns((Data.Models.Author?)null);
        
        Assert.False(authorSvc.UpdateAuthor(Setup.Mapper.Map<TheatreHolic.Data.Models.Author, Author>(author)));
    }
    
    [Fact]
    public void TitleIsWhitespace_ReturnFalse()
    {
        var authorSvc = GetService();
        var author = new Data.Models.Author();
        author.Id = 10;
        author.Name = "     ";
        
        Setup.AuthorRepository.Setup(s => s.Find(author.Id)).Returns(author);
        
        Assert.False(authorSvc.UpdateAuthor(Setup.Mapper.Map<TheatreHolic.Data.Models.Author, Author>(author)));
    }
    
    private IAuthorService GetService()
    {
        return new Domain.Services.Impl.AuthorService(Setup.AuthorRepository.Object, Setup.Mapper);
    }
}