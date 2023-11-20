namespace WeatherApp.Application.Models;

public class HighestWindSpeedModel
{
    public string CountryName { get; set; }

    public string CityName { get; set; }

    public double HighestWind { get; set; }

    public DateTime LastUpdate { get; set; }
}
