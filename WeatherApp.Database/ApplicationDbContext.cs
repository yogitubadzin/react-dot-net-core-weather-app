using Microsoft.EntityFrameworkCore;
using WeatherApp.Core.Models;
using WeatherApp.Database.Configurations;
using WeatherApp.Database.Seeds;

namespace WeatherApp.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<City> Cities{ get; set; }

    public DbSet<CityWeather> CityWeathers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CityWeatherConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());

        modelBuilder.Entity<City>().HasData(CitiesProvider.Get());

        EnsureStoredProceduresAreCreated();
    }

    private void EnsureStoredProceduresAreCreated()
    {
        var getMinTempMaxWindSpeedForCountryAndTemperatureStoredProcedureScript = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[weather].[GetMinTempMaxWindSpeedForCountryAndTemperature]') AND type in (N'P', N'PC'))
            BEGIN
                CREATE PROCEDURE [weather].[GetMinTempMaxWindSpeedForCountryAndTemperature]
                    @CountryName NVARCHAR(255),
                    @MinTemperature FLOAT
                AS
                BEGIN
                    SELECT
                        c.CountryName,
                        MIN(cw.Temperature) AS MinTemperature,
                        MAX(cw.WindSpeed) AS MaxWindSpeed
                    FROM
                        weather.CityWeathers AS cw
	                JOIN weather.Cities c ON c.Id = cw.CityId
                    WHERE
                        c.CountryName = @CountryName
                        AND cw.Temperature < @MinTemperature
                    GROUP BY
                        c.CountryName
                END;
                GO
            END";

        var getMaxWindSpeedForCountryStoredProcedureScript = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[weather].[GetMaxWindSpeedForCountry]') AND type in (N'P', N'PC'))
            BEGIN
                CREATE PROCEDURE [weather].[GetMaxWindSpeedForCountry]
                    @CountryName NVARCHAR(255),
                    @MinTemperature FLOAT
                AS
                BEGIN
                    SELECT
                        c.CountryName,
                        MAX(cw.WindSpeed) AS MaxWindSpeed
                    FROM
                        weather.CityWeathers AS cw
	                JOIN weather.Cities c ON c.Id = cw.CityId
                    WHERE
                        c.CountryName = @CountryName
                    GROUP BY
                        c.CountryName
                END;
                GO
            END";

        ////Database.ExecuteSqlRaw(getMinTempMaxWindSpeedForCountryAndTemperatureStoredProcedureScript);
        ////Database.ExecuteSqlRaw(getMaxWindSpeedForCountryStoredProcedureScript);
    }
}
