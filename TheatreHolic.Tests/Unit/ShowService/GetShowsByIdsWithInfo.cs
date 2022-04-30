using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.ShowService;

public class GetShowsByIdsWithInfo
{
    [Fact]
    public void IdsAreEmpty_GetAllData()
    {
        var showSvc = GetService();

        var shows = Fixtures.Shows;
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.GetPageWithData(-1, -1)).Returns(shows);
        var res = showSvc.GetShowsByIdsWithInfo(new List<int>(), -1, -1);

        res.Should().BeEquivalentTo(showsDomain);

        Setup.ShowRepository.Setup(s => s.GetPageWithData(2, 2)).Returns(shows.Skip(2).Take(2).ToList());
        res = showSvc.GetShowsByIdsWithInfo(new List<int>(), 2, 2);

        res.Should().BeEquivalentTo(showsDomain.Skip(2).Take(2).ToList());
    }
    
    [Fact]
    public void Success()
    {
        var showSvc = GetService();

        var ids = new List<int> { 2, 3, 4, 5 };

        var shows = Fixtures.Shows.Where(s => ids.Contains(s.Id)).ToList();
        var showsDomain = shows.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(s)).ToList();

        Setup.ShowRepository.Setup(s => s.FilterWithData(show => ids.Contains(show.Id), -1, -1)).Returns(shows);
        var res = showSvc.GetShowsByIdsWithInfo(ids, -1, -1);

        res.Should().BeEquivalentTo(showsDomain);
        
        Setup.ShowRepository.Setup(s => s.FilterWithData(show => ids.Contains(show.Id), 2, 1)).Returns(shows.Skip(2).Take(1).ToList());
        res = showSvc.GetShowsByIdsWithInfo(ids, 2, 1);

        res.Should().BeEquivalentTo(showsDomain.Skip(2).Take(1).ToList());
    }
    
    private IShowService GetService()
    {
        return new Domain.Services.Impl.ShowService(Setup.ShowRepository.Object, Setup.Mapper);
    }
}