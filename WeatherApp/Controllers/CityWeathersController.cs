using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services.Interfaces;

namespace WeatherApp.Controllers;

[ApiController]
[Route("cities/weathers")]
public class CityWeathersController : ControllerBase
{
    private readonly ICityWeatherService _cityWeatherService;

    public CityWeathersController(ICityWeatherService cityWeatherService)
    {
        _cityWeatherService = cityWeatherService;
    }

    [HttpGet("lowest-temperatures")]
    public async Task<List<LowestTemperatureModel>> GetLowestTemperatureModels()
    {
        return await _cityWeatherService.GetLowestTemperatureModels();
    }

    [HttpGet("highest-wind-speeds")]
    public async Task<List<HighestWindSpeedModel>> GetHighestWindSpeedModels()
    {
        return await _cityWeatherService.GetHighestWindSpeedModels();
    }
}
