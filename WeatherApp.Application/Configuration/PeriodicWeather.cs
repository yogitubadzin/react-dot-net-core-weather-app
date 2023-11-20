namespace WeatherApp.Application.Configuration;

public sealed record PeriodicWeather
{
    public string ApiKey { get; set; }

    public string Url { get; set; }

    public int FromMinutes { get; set; }
}
