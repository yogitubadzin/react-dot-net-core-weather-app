using WeatherApp.Core.CommonServices.Interfaces;

namespace WeatherApp.Core.CommonServices.Implementation;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow { get; } = DateTime.UtcNow;
}
