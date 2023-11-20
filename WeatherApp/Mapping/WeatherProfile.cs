using AutoMapper;
using WeatherApp.Application.Models;
using WeatherApp.Core.Models;

namespace WeatherApp.Mapping;

public class WeatherProfile : Profile
{
    public WeatherProfile()
    {
        CreateMap<CityWeatherResult, CityWeather>();
    }
}
