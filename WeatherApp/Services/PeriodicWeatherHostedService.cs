using WeatherApp.Application.Configuration;
using WeatherApp.Application.Services.Interfaces;

namespace WeatherApp.Services;

public class PeriodicWeatherHostedService : BackgroundService
{
    private readonly TimeSpan _period;
    private readonly IServiceScopeFactory _scopeScopeFactory;

    public PeriodicWeatherHostedService(
        IServiceScopeFactory scopeFactory,
        PeriodicWeather periodicWeather)
    {
        _scopeScopeFactory = scopeFactory;
        _period = TimeSpan.FromMinutes(periodicWeather.FromMinutes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = _scopeScopeFactory.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<IPeriodicWeatherService>();

            await service.Fetch();
        }
    }
}
