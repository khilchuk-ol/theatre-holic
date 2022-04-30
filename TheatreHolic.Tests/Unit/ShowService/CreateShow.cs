using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.ShowService;

public class CreateShow
{
    [Fact]
    public void Success()
    {
        var showSvc = GetService();
        var show = Fixtures.Shows[0];
        
        show.Tickets = null;
        show.Author = null;
        show.Genre = null;
        
        Setup.ShowRepository.Setup(s => s.Create(show));
        showSvc.CreateShow(Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(show));
    }
    
    private IShowService GetService()
    {
        return new Domain.Services.Impl.ShowService(Setup.ShowRepository.Object, Setup.Mapper);
    }
}