using WeatherApp.Application.Models;
using WeatherApp.Application.Services.Interfaces;
using WeatherApp.Core.Interfaces.Repositories;

namespace WeatherApp.Application.Services.Implementation;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;

    public CityService(ICityRepository cityRepository)
    {
        this._cityRepository = cityRepository;
    }

    public async Task<List<CityModel>> Get()
    {
        var cities = await _cityRepository.GetAllAsync();

        return cities.Select(x => new CityModel
        {
            CityName = x.Name,
            CountryName = x.CountryName
        }).ToList();
    }
}
