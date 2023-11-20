namespace WeatherApp.Application.Utils.Implementation;

public class UrlQueryBuilder
{
    private string _url;

    private UrlQueryBuilder(string url)
    {
        _url = url;
    }

    public static UrlQueryBuilder Create(string url)
    {
        return new UrlQueryBuilder(url);
    }

    public UrlQueryBuilder WithCityAndCountryCode(string city, string country)
    {
        _url = $"{_url}weather?q={city},{country}";

        return this;
    }

    public UrlQueryBuilder WithManyCities(List<int> cityIds)
    {
        _url = $"{_url}group?id={string.Join(",", cityIds)}";

        return this;
    }

    public UrlQueryBuilder WithAppId(string appId)
    {
        _url = $"{_url}&appId={appId}";

        return this;
    }

    public string Build()
    {
        return _url;
    }
}
