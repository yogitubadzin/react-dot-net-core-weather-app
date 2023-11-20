using WeatherApp.Application.Models;

namespace WeatherApp.Application.Services.Interfaces;

public interface ICityWeatherService
{
    Task<List<LowestTemperatureModel>> GetLowestTemperatureModels();

    Task<List<HighestWindSpeedModel>> GetHighestWindSpeedModels();
}
