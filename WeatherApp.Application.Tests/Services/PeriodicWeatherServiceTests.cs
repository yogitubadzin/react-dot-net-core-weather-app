using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WeatherApp.Application.Models;
using WeatherApp.Application.Services.Implementation;
using WeatherApp.Application.Services.Interfaces;
using WeatherApp.Application.Utils.Interfaces;
using WeatherApp.Core.CommonServices.Interfaces;
using WeatherApp.Core.CommonServices.Models;
using WeatherApp.Core.Interfaces.Repositories;
using WeatherApp.Core.Models;

namespace WeatherApp.Application.Tests.Services;

public class PeriodicWeatherServiceTests
{
    private IFixture _fixture;
    private Mock<IHttpClientService> _htpClientServiceMock;
    private Mock<ICityWeatherRepository> _cityWeatherRepositoryMock;
    private Mock<IDateTimeService> _dateTimeServiceMock;
    private Mock<IKelvinToCelciusConverter> _kelvinToCelciusConverterMock;
    private Mock<ICityRepository> _cityRepositoryMock;

    private IPeriodicWeatherService _periodicWeatherService;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _htpClientServiceMock = _fixture.Freeze<Mock<IHttpClientService>>();
        _cityWeatherRepositoryMock = _fixture.Freeze<Mock<ICityWeatherRepository>>();
        _dateTimeServiceMock = _fixture.Freeze<Mock<IDateTimeService>>();
        _kelvinToCelciusConverterMock = _fixture.Freeze<Mock<IKelvinToCelciusConverter>>();
        _cityRepositoryMock = _fixture.Freeze<Mock<ICityRepository>>();

        _periodicWeatherService = _fixture.Create<PeriodicWeatherService>();

    }

    [Test]
    public async Task Fetch_ForCityWithoutExternalId_ShouldFetchWeatherLogs()
    {
        // Arrange
        var city = new City
        {
            Id = Guid.NewGuid(),
            Name = "Miami",
            CountryCode = "US",
            CountryName = "USA",
        };

        var temperature = 22.3;

        var cityWeatherResult = _fixture.Create<HttpResponseResult<CityWeatherResult>>();

        _cityRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<City> { city });

        _htpClientServiceMock
            .Setup(client => client.GetAsync<CityWeatherResult>(It.IsAny<string>()))
            .ReturnsAsync(cityWeatherResult);

        var now = DateTime.Now;

        _dateTimeServiceMock
            .Setup(x => x.UtcNow)
            .Returns(now);

        SetUpKelvinToCelciusConverterMock(cityWeatherResult.Result, temperature);

        List<CityWeather> cityWeathersResult = null;
        _cityWeatherRepositoryMock
            .Setup(repo => repo.AddRangeAsync(It.IsAny<List<CityWeather>>()))
            .Callback<List<CityWeather>>(cityWeathers =>
            {
                cityWeathersResult = cityWeathers;
            });

        // Act
        await _periodicWeatherService.Fetch();

        // Assert
        _cityWeatherRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);

        cityWeathersResult.Should().NotBeNull();
        cityWeathersResult.Should().HaveCount(1);

        cityWeathersResult[0].City.Should().Be(city);
        cityWeathersResult[0].Temperature.Should().Be(temperature);
        cityWeathersResult[0].WindSpeed.Should().Be(cityWeatherResult.Result.Wind.Speed);
        cityWeathersResult[0].LastUpdate.Should().Be(now);
    }

    [Test]
    public async Task Fetch_ForCityWithExternalId_ShouldFetchWeatherLogs()
    {
        // Arrange
        var temperature1 = 22.3;
        var temperature2 = 23.3;
        var temperature3 = 24.3;

        var cityWeatherResult = _fixture.Create<HttpResponseResult<CityWeatherResults>>();
        var cities = new List<City>
        {
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Miami",
                CountryCode = "US",
                CountryName = "USA",
                ExternalId = cityWeatherResult.Result.List[0].Id,
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Seattle",
                CountryCode = "US",
                CountryName = "USA",
                ExternalId = cityWeatherResult.Result.List[1].Id,
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Barcelona",
                CountryCode = "ES",
                CountryName = "Spain",
                ExternalId = cityWeatherResult.Result.List[2].Id,
            }
        };


        _cityRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(cities);

        _htpClientServiceMock
            .Setup(client => client.GetAsync<CityWeatherResults>(It.IsAny<string>()))
            .ReturnsAsync(cityWeatherResult);

        var now = DateTime.Now;

        _dateTimeServiceMock
            .Setup(x => x.UtcNow)
            .Returns(now);

        SetUpKelvinToCelciusConverterMock(cityWeatherResult.Result.List[0], temperature1);
        SetUpKelvinToCelciusConverterMock(cityWeatherResult.Result.List[1], temperature2);
        SetUpKelvinToCelciusConverterMock(cityWeatherResult.Result.List[2], temperature3);

        List<CityWeather> cityWeathersResult = null;
        _cityWeatherRepositoryMock
            .Setup(repo => repo.AddRangeAsync(It.IsAny<List<CityWeather>>()))
            .Callback<List<CityWeather>>(cityWeathers =>
            {
                cityWeathersResult = cityWeathers;
            });

        // Act
        await _periodicWeatherService.Fetch();

        // Assert
        _cityWeatherRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);

        cityWeathersResult.Should().NotBeNull();
        cityWeathersResult.Should().HaveCount(3);

        cityWeathersResult[0].City.Should().Be(cities[0]);
        cityWeathersResult[0].Temperature.Should().Be(temperature1);
        cityWeathersResult[0].WindSpeed.Should().Be(cityWeatherResult.Result.List[0].Wind.Speed);
        cityWeathersResult[0].LastUpdate.Should().Be(now);

        cityWeathersResult[1].City.Should().Be(cities[1]);
        cityWeathersResult[1].Temperature.Should().Be(temperature2);
        cityWeathersResult[1].WindSpeed.Should().Be(cityWeatherResult.Result.List[1].Wind.Speed);
        cityWeathersResult[1].LastUpdate.Should().Be(now);

        cityWeathersResult[2].City.Should().Be(cities[2]);
        cityWeathersResult[2].Temperature.Should().Be(temperature3);
        cityWeathersResult[2].WindSpeed.Should().Be(cityWeatherResult.Result.List[2].Wind.Speed);
        cityWeathersResult[2].LastUpdate.Should().Be(now);
    }

    private void SetUpKelvinToCelciusConverterMock(CityWeatherResult cityWeatherResult, double temperature)
    {
        _kelvinToCelciusConverterMock
            .Setup(x => x.Convert(cityWeatherResult.Main.Temp))
            .Returns(temperature);
    }
}