namespace WeatherApp.Core.Models;

public record City
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string CountryName { get; set; }

    public string CountryCode { get; set; }

    public int? ExternalId { get; set; }

    public List<CityWeather> Weathers { get; set; }
}
