using WeatherApp.Core.Models;

namespace WeatherApp.Core.Interfaces.Repositories;

public interface ICityWeatherRepository
{
    Task AddRangeAsync(List<CityWeather> cityWeathers);

    Task SaveAsync();
}
