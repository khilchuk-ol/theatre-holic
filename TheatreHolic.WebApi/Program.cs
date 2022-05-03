using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheatreHolic.Data;
using TheatreHolic.Data.Repository;
using TheatreHolic.Data.Repository.Impl;
using TheatreHolic.Data.UnitOfWork;
using TheatreHolic.Domain.Services;
using TheatreHolic.Domain.Services.Impl;
using MappingProfile = TheatreHolic.WebApi.Mapping.MappingProfile;

namespace TheatreHolic.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        ConfigureServices(builder.Services);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
    
    public static void ConfigureServices(IServiceCollection services)
    {
        // services.AddDbContext<TheatreHolicContext>();

        services.AddScoped<DbContext, TheatreHolicContext>();

        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IShowRepository, ShowRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IShowService, ShowService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IGenreService, GenreService>();
        
        services.AddScoped<UnitOfWork, Data.UnitOfWork.Impl.UnitOfWork>();
        
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
            mc.AddProfile(new Domain.Mapping.MappingProfile());
        });
        
        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
}