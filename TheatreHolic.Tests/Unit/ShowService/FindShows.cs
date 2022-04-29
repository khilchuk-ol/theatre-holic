using System.Linq;
using FluentAssertions;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using TheatreHolic.Domain.Services.Utils;
using Xunit;

namespace TheatreHolic.Tests.Unit.ShowService;

public class FindShows
{
    [Fact]
    public void OptionsAreNull_GetAllData()
    {
        var showSvc = GetService();
        
        var shows = Fixtures.Shows;
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.GetPage(-1, -1)).Returns(shows);
        var res = showSvc.FindShows(null, -1, -1);
        
        res.Should().BeEquivalentTo(showsDomain);

        Setup.ShowRepository.Setup(s => s.GetPage(2, 2)).Returns(shows.Skip(2).Take(2).ToList());
        res = showSvc.FindShows(null, 2, 2);
        
        res.Should().BeEquivalentTo(showsDomain.Skip(2).Take(2).ToList());
    }

    [Fact]
    public void FilterIsEmpty_GetAllData()
    {
        var showSvc = GetService();
        
        var shows = Fixtures.Shows;
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.GetPage(-1, -1)).Returns(shows);
        var res = showSvc.FindShows(new SearchShowsOptions(), -1, -1);

        res.Should().BeEquivalentTo(showsDomain);

        Setup.ShowRepository.Setup(s => s.GetPage(2, 2)).Returns(shows.Skip(2).Take(2).ToList());
        res = showSvc.FindShows(new SearchShowsOptions(), 2, 2);

        res.Should().BeEquivalentTo(showsDomain.Skip(2).Take(2).ToList());
    }

    private IShowService GetService()
    {
        return new Domain.Services.Impl.ShowService(Setup.ShowRepository.Object, Setup.Mapper);
    }
}