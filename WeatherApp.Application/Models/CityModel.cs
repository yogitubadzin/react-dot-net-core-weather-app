namespace WeatherApp.Application.Models;

public sealed record CityModel
{
    public string CityName { get; set; }

    public string CountryName { get; set; }
}
