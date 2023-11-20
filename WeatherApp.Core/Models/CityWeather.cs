namespace WeatherApp.Core.Models;

public class CityWeather
{
    public Guid Id { get; set; }

    public double Temperature { get; set; }

    public double WindSpeed { get; set; }

    public DateTime LastUpdate { get; set; }

    public City City { get; set; }
}
