using System;
using Microsoft.Extensions.Logging;
using Moq;
using TheatreHolic.Data.Exceptions;
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
        var show = new Data.Models.Show()
        {
            Title = "title",
            Date = DateTime.Now.AddDays(3),
            AuthorId = 10,
            Author = new Data.Models.Author
            {
                Id = 10
            },
            GenreId = 10,
            Genre = new Data.Models.Genre
            {
                Id = 10
            },
        };

        Setup.ShowRepository.Setup(s => s.Create(It.Is(
            (Data.Models.Show d) => d.Equals(show))));
        Assert.True(showSvc.CreateShow(Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(show)));
    }

    [Fact]
    public void DateIsInvalid_ReturnFalse()
    {
        var showSvc = GetService();
        var show = new Data.Models.Show
        {
            Date = DateTime.Now.AddDays(-2)
        };

        Assert.False(showSvc.CreateShow(Setup.Mapper.Map<TheatreHolic.Data.Models.Show, Show>(show)));
    }

    [Fact]
    public void RepositoryThrowException_ReturnFalse()
    {
        var showSvc = GetService();
        var show = new Show
        {
            Title = "title",
            Date = DateTime.Now.AddDays(3),
            Author = new Author { Id = 10 },
            Genre = new Genre { Id = 10 }
        };

        var model = Setup.Mapper.Map<Show, Data.Models.Show>(show);
        Setup.ShowRepository.Setup(s => s.Create(It.Is(
                (Data.Models.Show d) => d.Equals(model))))
            .Throws(new InvalidForeignKeyException("msg"));
        Assert.False(showSvc.CreateShow(show));
    }

    private IShowService GetService()
    {
        return new Domain.Services.Impl.ShowService(Setup.ShowRepository.Object, Setup.Mapper,
            new Mock<ILogger<Domain.Services.Impl.ShowService>>().Object);
    }
}