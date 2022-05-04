using System;
using Microsoft.Extensions.Logging;
using Moq;
using TheatreHolic.Data.Exceptions;
using TheatreHolic.Data.Models;
using TheatreHolic.Domain.Services;
using Xunit;
using Show = TheatreHolic.Domain.Models.Show;

namespace TheatreHolic.Tests.Unit.ShowService;

public class UpdateShow
{
    [Fact]
    public void Success_ReturnTrue()
    {
        var showSvc = GetService();
        var show = new Data.Models.Show();
        show.Id = 10;
        
        show.Author = new Author { Id = 12 };
        show.AuthorId = 12;
        
        show.Genre = new Genre { Id = 12};
        show.GenreId = 12;

        show.Title = "new title";
        show.Date = DateTime.Now.AddDays(3);

        Setup.ShowRepository.Setup(s => s.Find(show.Id)).Returns(show);
        Setup.ShowRepository.Setup(s => s.Update(show));
        
        Assert.True(showSvc.UpdateShow(Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(show)));
    }
    
    [Fact]
    public void NoSuchItem_ReturnFalse()
    {
        var showSvc = GetService();
        var show = new Data.Models.Show();
        show.Id = 10;
        
        Setup.ShowRepository.Setup(s => s.Find(show.Id)).Returns((Data.Models.Show?)null);
        
        Assert.False(showSvc.UpdateShow(Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(show)));
    }
    
    [Fact]
    public void TitleIsWhitespace_ReturnFalse()
    {
        var showSvc = GetService();
        var show = new Data.Models.Show();
        show.Id = 10;
        
        show.Tickets = null;
        show.Author = null;
        show.Genre = null;

        show.Title = "      ";
        show.Date = DateTime.Now.AddDays(3);
        
        Setup.ShowRepository.Setup(s => s.Find(show.Id)).Returns(show);

        Assert.False(showSvc.UpdateShow(Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(show)));
    }
    
    [Fact]
    public void DateIsPast_ReturnFalse()
    {
        var showSvc = GetService();
        var show = new Data.Models.Show();
        show.Id = 10;
        
        show.Tickets = null;
        show.Author = null;
        show.Genre = null;

        show.Title = "new title";
        show.Date = DateTime.Now.AddDays(-3);
        
        Setup.ShowRepository.Setup(s => s.Find(show.Id)).Returns(show);

        Assert.False(showSvc.UpdateShow(Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(show)));
    }
    
    [Fact]
    public void RepositoryThrowException_ReturnFalse()
    {
        var showSvc = GetService();
        var show = new Data.Models.Show();
        show.Id = 10;
        
        show.Tickets = null;
        show.Author = null;
        show.Genre = null;

        show.Title = "new title";
        show.Date = DateTime.Now.AddDays(3);
        
        Setup.ShowRepository.Setup(s => s.Find(show.Id)).Returns(show);

        Setup.ShowRepository.Setup(s => s.Update(show)).Throws(new InvalidForeignKeyException("msg"));
        Assert.False(showSvc.UpdateShow(Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(show)));
    }
    
    private IShowService GetService()
    {
        return new Domain.Services.Impl.ShowService(Setup.ShowRepository.Object, Setup.Mapper,
            new Mock<ILogger<Domain.Services.Impl.ShowService>>().Object);
    }
}