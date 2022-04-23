using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TheatreHolic.Data;
using TheatreHolic.Data.Repository;
using TheatreHolic.Data.Repository.Impl;
using TheatreHolic.Domain.Mapping;
using TheatreHolic.Domain.Services;
using TheatreHolic.Domain.Services.Impl;

namespace TheatreHolic.CLI;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<TheatreHolicContext>();

        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IShowRepository, ShowRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IShowService, ShowService>();

        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        
        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
}