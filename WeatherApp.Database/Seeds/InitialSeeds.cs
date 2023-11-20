using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WeatherApp.Database.Seeds;

public static class InitialSeeds
{
    public static void Seed(DatabaseFacade database)
    {
        var getMinTempMaxWindSpeedForCountryAndTemperatureStoredProcedureScript = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[weather].[GetMinTempMaxWindSpeedForMinTemperature]') AND type in (N'P', N'PC'))
            BEGIN
                EXECUTE('
				CREATE PROCEDURE [weather].[GetMinTempMaxWindSpeedForMinTemperature]
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
                        cw.Temperature < @MinTemperature
                    GROUP BY
                        c.CountryName
                END')
            END";

        var getMaxWindSpeedForCountryStoredProcedureScript = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[weather].[GetMaxWindSpeedForCountry]') AND type in (N'P', N'PC'))
            BEGIN
                EXECUTE('
				CREATE PROCEDURE [weather].[GetMaxWindSpeedForCountry]
                    @CountryName NVARCHAR(255)
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
                END;')
            END";

        database.ExecuteSqlRaw(getMinTempMaxWindSpeedForCountryAndTemperatureStoredProcedureScript);
        database.ExecuteSqlRaw(getMaxWindSpeedForCountryStoredProcedureScript);
    }
}
