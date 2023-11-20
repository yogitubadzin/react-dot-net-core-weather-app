using Microsoft.EntityFrameworkCore;
using WeatherApp.Core.Interfaces.Repositories;
using WeatherApp.Core.Models;
using WeatherApp.Database;

namespace WeatherApp.DataAccess.Repositories;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<City>> GetAllAsync()
    {
        return await _context
            .Cities
            .Include(x => x.Weathers)
            .ToListAsync();
    }
}
