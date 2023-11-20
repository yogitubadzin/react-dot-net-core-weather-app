namespace WeatherApp.Application.Models;

public sealed record CityWeatherResult
{
    public int Id { get; set; }

    public string Name { get; set; }

    public CityWeatherMainResult Main { get; set; }

    public CityWeatherWindResult Wind { get; set; }

    public CityWeatherSysResult Sys { get; set; }
}
