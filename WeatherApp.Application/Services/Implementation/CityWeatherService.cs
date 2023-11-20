using WeatherApp.Application.Models;
using WeatherApp.Application.Services.Interfaces;
using WeatherApp.Core.CommonServices.Interfaces;
using WeatherApp.Core.Interfaces.Repositories;

namespace WeatherApp.Application.Services.Implementation;

public class CityWeatherService : ICityWeatherService
{
    private readonly ICityRepository _cityRepository;
    private readonly IDateTimeService _dateTimeService;

    public CityWeatherService(
        ICityRepository cityRepository,
        IDateTimeService dateTimeService)
    {
        this._cityRepository = cityRepository;
        _dateTimeService = dateTimeService;
    }

    public async Task<List<LowestTemperatureModel>> GetLowestTemperatureModels()
    {
        var startDateRange = _dateTimeService.UtcNow.AddHours(-2);
        var cities = await _cityRepository.GetAllAsync();

        var allWeathers = cities.SelectMany(x => x.Weathers).ToList();

        var minimalTemperatureModels = allWeathers
            .Where(x => x.LastUpdate >= startDateRange)
            .GroupBy(x => x.LastUpdate)
            .Select(x =>
            {
                var updateWithLowestTemperature = x.MinBy(y => y.Temperature);

                return new LowestTemperatureModel
                {
                    CountryName = updateWithLowestTemperature.City.CountryName,
                    CityName = updateWithLowestTemperature.City.Name,
                    Temperature = updateWithLowestTemperature.Temperature,
                    LastUpdate = updateWithLowestTemperature.LastUpdate
                };
            })
            .ToList();

        return minimalTemperatureModels;
    }

    public async Task<List<HighestWindSpeedModel>> GetHighestWindSpeedModels()
    {
        var startDateRange = _dateTimeService.UtcNow.AddHours(-2);
        var cities = await _cityRepository.GetAllAsync();

        var allWeathers = cities.SelectMany(x => x.Weathers).ToList();

        var highestWindSpeedModels = allWeathers
            .Where(x => x.LastUpdate >= startDateRange)
            .GroupBy(x => x.LastUpdate)
            .Select(x =>
            {
                var updateWithHighestWindSpeed = x.MaxBy(y => y.WindSpeed);

                return new HighestWindSpeedModel
                {
                    CountryName = updateWithHighestWindSpeed.City.CountryName,
                    CityName = updateWithHighestWindSpeed.City.Name,
                    HighestWind = updateWithHighestWindSpeed.WindSpeed,
                    LastUpdate = updateWithHighestWindSpeed.LastUpdate
                };
            })
            .ToList();

        return highestWindSpeedModels;
    }
}
