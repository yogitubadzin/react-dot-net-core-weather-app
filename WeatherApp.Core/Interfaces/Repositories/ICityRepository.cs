using WeatherApp.Core.Models;

namespace WeatherApp.Core.Interfaces.Repositories;

public interface ICityRepository
{
    Task<List<City>> GetAllAsync();
}
