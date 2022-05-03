using Microsoft.Extensions.Logging;
using Moq;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.ShowService;

public class DeleteShow
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var showSvc = GetService();
        var show = Fixtures.Shows[0];

        Setup.ShowRepository.Setup(s => s.Remove(show.Id)).Returns(true);
        Assert.True(showSvc.DeleteShow(show.Id));
    }
    
    [Fact]
    public void RepositoryReturnFalse_ReturnFalse()
    {
        var showSvc = GetService();
        var show = Fixtures.Shows[0];

        Setup.ShowRepository.Setup(s => s.Remove(show.Id)).Returns(false);
        Assert.False(showSvc.DeleteShow(show.Id));
    }

    private IShowService GetService()
    {
        return new Domain.Services.Impl.ShowService(Setup.ShowRepository.Object, Setup.Mapper,
            new Mock<ILogger<Domain.Services.Impl.ShowService>>().Object);
    }
}