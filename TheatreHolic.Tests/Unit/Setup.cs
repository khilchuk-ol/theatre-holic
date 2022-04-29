using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoMapper;
using Moq;
using TheatreHolic.Data.Models;
using TheatreHolic.Data.Repository;
using TheatreHolic.Data.UnitOfWork;
using TheatreHolic.Domain.Services;
using TheatreHolic.Tests.Utils.Mapping;

namespace TheatreHolic.Tests.Unit;

public static class Setup
{
    public static Mock<IShowRepository> ShowRepository { get; } = new();

    public static Mock<ITicketRepository> TicketRepository { get; } = new();

    public static Mock<IGenreRepository> GenreRepository { get; } = new();

    public static Mock<IAuthorRepository> AuthorRepository { get; } = new();
    
    public static IMapper Mapper { get; }
    
    static Setup()
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        
        Mapper = mapperConfig.CreateMapper();
    }
}