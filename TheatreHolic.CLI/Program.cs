using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheatreHolic.CLI.Presentation;
using TheatreHolic.Data;
using TheatreHolic.Data.Repository;
using TheatreHolic.Data.Repository.Impl;
using TheatreHolic.Domain.Mapping;
using TheatreHolic.Domain.Services;
using TheatreHolic.Domain.Services.Impl;

namespace TheatreHolic.CLI;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            try
            {
                var cli = services.GetRequiredService<IShowable>();
                
                cli.Show();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred.");
            }
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                Startup.ConfigureServices(services);
            });
}