using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.ShowService;

public class DeleteShow
{
    [Fact]
    public void Success()
    {
        var showSvc = GetService();
        var show = Fixtures.Shows[0];
        
        show.Tickets = null;
        show.Author = null;
        show.Genre = null;
        
        Setup.ShowRepository.Setup(s => s.Remove(show.Id));
        showSvc.DeleteShow(show.Id);
    }
    
    private IShowService GetService()
    {
        return new Domain.Services.Impl.ShowService(Setup.ShowRepository.Object, Setup.Mapper);
    }
}