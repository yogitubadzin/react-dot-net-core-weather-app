using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherApp.Core.Models;

namespace WeatherApp.Database.Configurations;

public class CityConfiguration : ConfigurationBase<City>
{
    protected override string EntityName => nameof(ApplicationDbContext.Cities);

    protected override void ConfigureEntity(EntityTypeBuilder<City> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(e => e.CountryName)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(e => e.CountryCode)
            .IsRequired()
            .HasMaxLength(2);

        builder
            .Property(e => e.ExternalId);

        builder
            .HasMany<CityWeather>(e => e.Weathers);
    }

    protected override void ConfigureEntityType(EntityTypeBuilder<City> builder)
    {
        builder.ToTable(EntityName, SchemaName);
    }
}
