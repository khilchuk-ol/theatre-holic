using Microsoft.Extensions.Logging;
using Moq;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.ShowService;

public class CreateShow
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var showSvc = GetService();
        var show = Fixtures.Shows[0];

        Setup.ShowRepository.Setup(s => s.Create(show));
        Assert.True(showSvc.CreateShow(Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(show)));
    }
    
    // [Fact]
    // public void RepositoryThrowException_ReturnFalse()
    // {
    //     var showSvc = GetService();
    //     var show = new Show
    //     {
    //         Title = "title",
    //         Date = DateTime.Now.AddDays(3),
    //         Author = new Author(){ Id = 10 },
    //         Genre = new Genre() { Id = 10 }
    //     };
    //
    //     var mappedShow = Setup.Mapper.Map<Show, TheatreHolic.Data.Models.Show>(show);
    //
    //     Setup.ShowRepository.Setup(s => s.Create(mappedShow)).Throws(new InvalidForeignKeyException("msg"));
    //     Assert.False(showSvc.CreateShow(show));
    // }
    
    private IShowService GetService()
    {
        return new Domain.Services.Impl.ShowService(Setup.ShowRepository.Object, Setup.Mapper,
            new Mock<ILogger<Domain.Services.Impl.ShowService>>().Object);
    }
}