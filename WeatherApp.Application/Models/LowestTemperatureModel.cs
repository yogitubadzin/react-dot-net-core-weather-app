namespace WeatherApp.Application.Models;

public class LowestTemperatureModel
{
    public string CountryName { get; set; }

    public string CityName { get; set; }

    public double Temperature { get; set; }

    public DateTime LastUpdate { get; set; }
}