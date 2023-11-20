using WeatherApp.Application.Models;

namespace WeatherApp.Application.Services.Interfaces;

public interface ICityService
{
    Task<List<CityModel>> Get();
}
