using WeatherApp.Core.CommonServices.Models;

namespace WeatherApp.Core.CommonServices.Interfaces;

public interface IHttpClientService
{
    Task<HttpResponseResult<T>> GetAsync<T>(string url)
        where T : class;
}
