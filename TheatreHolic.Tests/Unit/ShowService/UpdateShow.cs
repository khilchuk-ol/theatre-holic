using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.ShowService;

public class UpdateShow
{
    [Fact]
    public void Success()
    {
        var showSvc = GetService();
        var show = Fixtures.Shows[0];
        
        show.Tickets = null;
        show.Author = null;
        show.Genre = null;

        show.Title = "new title";
        
        Setup.ShowRepository.Setup(s => s.Update(show));
        showSvc.UpdateShow(Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(show));
    }
    
    private IShowService GetService()
    {
        return new Domain.Services.Impl.ShowService(Setup.ShowRepository.Object, Setup.Mapper);
    }
}