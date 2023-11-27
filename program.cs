using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddScoped<IConfiguration>(_ => configuration)
            .AddScoped<AggregationJob>()
            .AddScoped<Scheduler>()
            .BuildServiceProvider();

        var scheduler = serviceProvider.GetService<Scheduler>();
        await scheduler.StartAsync();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        await scheduler.StopAsync();
    }
}
