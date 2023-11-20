using WeatherApp.Core.Interfaces.Repositories;
using WeatherApp.Core.Models;
using WeatherApp.Database;

namespace WeatherApp.DataAccess.Repositories;

public class CityWeatherRepository : ICityWeatherRepository
{
    private readonly ApplicationDbContext _context;

    public CityWeatherRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(List<CityWeather> cityWeathers)
    {
        await _context.AddRangeAsync(cityWeathers);
    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}
