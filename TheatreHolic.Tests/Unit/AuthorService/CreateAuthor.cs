using Moq;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.AuthorService;

public class CreateAuthor
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var authorSvc = GetService();
        var author = new Data.Models.Author
        {
            Name = "new author"
        };

        Setup.AuthorRepository.Setup(s => s.Create(It.Is(
            (Data.Models.Author d) => d.Equals(author))));
        Assert.True(authorSvc.CreateAuthor(Setup.Mapper.Map<TheatreHolic.Data.Models.Author, Author>(author)));
    }

    [Fact]
    public void NameIsWhitespace_ReturnFalse()
    {
        var authorSvc = GetService();
        var author = new Data.Models.Author
        {
            Name = "        "
        };

        Assert.False(authorSvc.CreateAuthor(Setup.Mapper.Map<TheatreHolic.Data.Models.Author, Author>(author)));
    }

    private IAuthorService GetService()
    {
        return new Domain.Services.Impl.AuthorService(Setup.AuthorRepository.Object, Setup.Mapper);
    }
}