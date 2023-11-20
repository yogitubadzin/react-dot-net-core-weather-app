using WeatherApp.Application.Utils.Interfaces;

namespace WeatherApp.Application.Utils.Implementation;

public class KelvinToCelciusConverter : IKelvinToCelciusConverter
{
    public double Convert(double value)
    {
        return Math.Round(value - 273.15, 2);
    }
}
