using Lib;
using Microsoft.Extensions.Logging;

namespace App;

public interface IWeatherService
{
    WeatherInfo GetWeatherInfo(string city);
}

public class WeatherService : IWeatherService
{
    private int _counter = 0;
    private readonly ILogger<WeatherService> _logger;
    private readonly IConcurrentDictionary<string, WeatherInfo> _cache;

    public WeatherService(IConcurrentDictionary<string, WeatherInfo> cache, ILogger<WeatherService> logger)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public WeatherInfo GetWeatherInfo(string city) => _cache.GetOrAdd(city, SimulateLongRunningOperation);

    private WeatherInfo SimulateLongRunningOperation(string city)
    {
        _logger.LogTrace("Simulate getting weather info from external api for city {city}", city);
        var delay = Interlocked.Increment(ref _counter);
        Thread.Sleep(TimeSpan.FromSeconds(delay));
        var weatherInfo = new WeatherInfo
        {
            City = city,
            Wind = $"+{Random.Shared.Next(1, 10)} km/h",
            Temperature = $"+{Random.Shared.Next(1, 30)} °C",
        };
        return weatherInfo;
    }
}