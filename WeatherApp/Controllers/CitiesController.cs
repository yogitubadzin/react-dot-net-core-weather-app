using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services.Interfaces;

namespace WeatherApp.Controllers;

[ApiController]
[Route("[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;

    public CitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet]
    public async Task<List<CityModel>> Get()
    {
        return await _cityService.Get();
    }
}
