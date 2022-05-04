using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.AuthorService;

public class DeleteAuthor
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var authorSvc = GetService();
        var author = Fixtures.Authors[0];

        Setup.AuthorRepository.Setup(s => s.Remove(author.Id)).Returns(true);
        Assert.True(authorSvc.DeleteAuthor(author.Id));
    }
    
    [Fact]
    public void RepositoryReturnFalse_ReturnFalse()
    {
        var authorSvc = GetService();
        var author = Fixtures.Authors[0];

        Setup.AuthorRepository.Setup(s => s.Remove(author.Id)).Returns(false);
        Assert.False(authorSvc.DeleteAuthor(author.Id));
    }
    
    private IAuthorService GetService()
    {
        return new Domain.Services.Impl.AuthorService(Setup.AuthorRepository.Object, Setup.Mapper);
    }
}