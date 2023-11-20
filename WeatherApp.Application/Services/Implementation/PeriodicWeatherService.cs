using WeatherApp.Application.Configuration;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services.Interfaces;
using WeatherApp.Application.Utils.Implementation;
using WeatherApp.Application.Utils.Interfaces;
using WeatherApp.Core.CommonServices.Interfaces;
using WeatherApp.Core.CommonServices.Models;
using WeatherApp.Core.Interfaces.Repositories;
using WeatherApp.Core.Models;

namespace WeatherApp.Application.Services.Implementation;

public class PeriodicWeatherService : IPeriodicWeatherService
{
    private readonly PeriodicWeather _periodicWeather;
    private readonly IHttpClientService _httpClientService;
    private readonly ICityWeatherRepository _cityWeatherRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly IKelvinToCelciusConverter _kelvinToCelciusConverter;
    private readonly ICityRepository _cityRepository;

    public PeriodicWeatherService(
        PeriodicWeather periodicWeather,
        IHttpClientService httpClientService,
        ICityWeatherRepository cityWeatherRepository,
        IDateTimeService dateTimeService,
        IKelvinToCelciusConverter kelvinToCelciusConverter,
        ICityRepository cityRepository)
    {
        _periodicWeather = periodicWeather;
        _httpClientService = httpClientService;
        _cityWeatherRepository = cityWeatherRepository;
        _dateTimeService = dateTimeService;
        _kelvinToCelciusConverter = kelvinToCelciusConverter;
        _cityRepository = cityRepository;
    }

    public async Task Fetch()
    {
        var cities = await _cityRepository.GetAllAsync();
        var cityIds = new List<int>();
        var cityWeathers = new List<CityWeather>();
        foreach (var city in cities)
        {
            if (city.ExternalId.HasValue)
            {
                cityIds.Add(city.ExternalId.Value);
                continue;
            }

            var cityWeatherResult = await RequestSingleCity(city, cityWeathers);

            AssignExternalIdForCity(city, cityWeatherResult);
        }

        await RequestGroupOfCities(cityIds, cities, cityWeathers);

        if (cityWeathers.Any())
        {
            await _cityWeatherRepository.AddRangeAsync(cityWeathers);
            await _cityWeatherRepository.SaveAsync();
        }
    }

    private async Task<HttpResponseResult<CityWeatherResult>> RequestSingleCity(City city, List<CityWeather> cityWeathers)
    {
        var cityWeatherUrl = UrlQueryBuilder
            .Create(_periodicWeather.Url)
            .WithCityAndCountryCode(city.Name, city.CountryCode)
            .WithAppId(_periodicWeather.ApiKey)
            .Build();

        var cityWeatherResult = await _httpClientService.GetAsync<CityWeatherResult>(cityWeatherUrl);

        var cityWeather = new CityWeather
        {
            City = city,
            Temperature = _kelvinToCelciusConverter.Convert(cityWeatherResult.Result.Main.Temp),
            WindSpeed = cityWeatherResult.Result.Wind.Speed,
            LastUpdate = _dateTimeService.UtcNow
        };

        cityWeathers.Add(cityWeather);
        return cityWeatherResult;
    }

    private async Task RequestGroupOfCities(List<int> cityIds, List<City> cities, List<CityWeather> cityWeathers)
    {
        if (cityIds.Any())
        {
            var citiesWeatherUrl = UrlQueryBuilder
                .Create(_periodicWeather.Url)
                .WithManyCities(cityIds)
                .WithAppId(_periodicWeather.ApiKey)
                .Build();

            var cityWeatherResults = await _httpClientService.GetAsync<CityWeatherResults>(citiesWeatherUrl);

            foreach (var cityWeatherResult in cityWeatherResults.Result.List)
            {
                var city = cities.FirstOrDefault(c => c.ExternalId == cityWeatherResult.Id);

                var cityWeather = new CityWeather
                {
                    City = city,
                    Temperature = _kelvinToCelciusConverter.Convert(cityWeatherResult.Main.Temp),
                    WindSpeed = cityWeatherResult.Wind.Speed,
                    LastUpdate = _dateTimeService.UtcNow
                };

                cityWeathers.Add(cityWeather);
            }
        }
    }

    private static void AssignExternalIdForCity(City city, HttpResponseResult<CityWeatherResult> cityWeatherResult)
    {
        city.ExternalId ??= cityWeatherResult.Result.Id;
    }
}
