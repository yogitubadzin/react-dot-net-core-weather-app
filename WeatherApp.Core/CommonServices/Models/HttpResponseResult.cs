namespace WeatherApp.Core.CommonServices.Models;

public class HttpResponseResult<T>
    where T : class
{
    public T Result { get; set; }

    public HttpResponseError Error { get; set; }
}
