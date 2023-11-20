using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherApp.Core.Models;

namespace WeatherApp.Database.Configurations;

public class CityWeatherConfiguration : ConfigurationBase<CityWeather>
{
    protected override string EntityName => nameof(ApplicationDbContext.CityWeathers);

    protected override void ConfigureEntity(EntityTypeBuilder<CityWeather> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Temperature);

        builder
            .Property(e => e.WindSpeed);

        builder
            .Property(e => e.LastUpdate);

        builder
            .HasOne<City>(e => e.City);
    }

    protected override void ConfigureEntityType(EntityTypeBuilder<CityWeather> builder)
    {
        builder.ToTable(EntityName, SchemaName);
    }
}
