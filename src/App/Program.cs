using System.Diagnostics;
using App.Extensions;
using Lib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App;

public static class Program
{
    public static void Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        var logger = host.Services.GetRequiredService<ILogger<WeatherService>>();
        var defaultConcurrentDictionary = host.Services.GetRequiredService<DefaultConcurrentDictionary<string, WeatherInfo>>();
        RunConcurrentDictionary(defaultConcurrentDictionary, logger);
        var lazyConcurrentDictionary = host.Services.GetRequiredService<LazyConcurrentDictionary<string, WeatherInfo>>();
        RunConcurrentDictionary(lazyConcurrentDictionary, logger);
        Console.WriteLine("Press any key to exit !");
        Console.ReadKey();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile();
                config.AddUserSecrets();
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureLogging((_, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddDefaultLogger();
            })
            .ConfigureServices((_, services) =>
            {
                services.AddTransient(typeof(LazyConcurrentDictionary<,>));
                services.AddTransient(typeof(DefaultConcurrentDictionary<,>));
            })
            .AddSerilog();

    private static void RunConcurrentDictionary(IConcurrentDictionary<string, WeatherInfo> concurrentDictionary, ILogger<WeatherService> logger, string city = "xyz", int retries = 5)
    {
        var name = concurrentDictionary is DefaultConcurrentDictionary<string, WeatherInfo> ? "default" : "lazy";
        logger.LogInformation("Running {name} concurrent dictionary", name);
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var weatherService = new WeatherService(concurrentDictionary, logger);
        var tasks = Enumerable.Range(0, retries)
            .Select(_ => Task.Run(() => { weatherService.GetWeatherInfo(city); }))
            .ToArray();
        Task.WaitAll(tasks);
        stopWatch.Stop();
        logger.LogInformation("Elapsed time = {time}", stopWatch.Elapsed.ToString("mm\\:ss\\.ff"));
    }
}