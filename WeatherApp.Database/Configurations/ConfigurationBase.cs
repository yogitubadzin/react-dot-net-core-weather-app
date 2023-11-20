using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeatherApp.Database.Configurations;

public abstract class ConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    protected const string SchemaName = "weather";
    protected abstract string EntityName { get; }

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ConfigureEntity(builder);
        ConfigureEntityType(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    protected abstract void ConfigureEntityType(EntityTypeBuilder<TEntity> builder);
}