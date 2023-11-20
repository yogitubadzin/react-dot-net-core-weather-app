using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WeatherApp.Application.Services.Implementation;
using WeatherApp.Application.Services.Interfaces;
using WeatherApp.Core.CommonServices.Interfaces;
using WeatherApp.Core.Interfaces.Repositories;
using WeatherApp.Core.Models;

namespace WeatherApp.Application.Tests.Services;
internal class CityWeatherServiceTests
{
    private IFixture _fixture;
    private Mock<IDateTimeService> _dateTimeServiceMock;
    private Mock<ICityRepository> _cityRepositoryMock;

    private ICityWeatherService _cityWeatherService;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _dateTimeServiceMock = _fixture.Freeze<Mock<IDateTimeService>>();
        _cityRepositoryMock = _fixture.Freeze<Mock<ICityRepository>>();

        _cityWeatherService = _fixture.Create<CityWeatherService>();
    }

    [Test]
    public async Task GetLowestTemperatureModels_ForLastUpdates_ShouldGet()
    {
        // Arrange
        var now = DateTime.Now;
        _dateTimeServiceMock
            .Setup(x => x.UtcNow)
            .Returns(now);

        var miamiCity = new City
        {
            CountryName = "USA",
            Name = "Miami"
        };

        var miamiCityWeathers = new List<CityWeather>
        {
            new CityWeather { Temperature = 25, LastUpdate = now.AddMinutes(-1), City = miamiCity },
            new CityWeather { Temperature = 25, LastUpdate = now.AddMinutes(-2), City = miamiCity },
            new CityWeather { Temperature = 25, LastUpdate = now.AddMinutes(-3), City = miamiCity },
        };

        miamiCity.Weathers = miamiCityWeathers;

        var barcelonaCity = new City
        {
            CountryName = "Spain",
            Name = "Barcelona",

        };

        var barcelonaCityWeathers = new List<CityWeather>
        {
            new CityWeather { Temperature = 26, LastUpdate = now.AddMinutes(-1), City = barcelonaCity },
            new CityWeather { Temperature = 24, LastUpdate = now.AddMinutes(-2), City = barcelonaCity },
            new CityWeather { Temperature = 23, LastUpdate = now.AddMinutes(-3), City = barcelonaCity },
        };

        barcelonaCity.Weathers = barcelonaCityWeathers;

        var cities = new List<City>
        {
            miamiCity,
            barcelonaCity
        };

        cities[0].Weathers[0].City = cities[0];

        _cityRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(cities);

        // Act
        var result = await _cityWeatherService.GetLowestTemperatureModels();

        // Assert
        result.Should().HaveCount(3);

        result[0].CountryName.Should().Be(miamiCity.CountryName);
        result[0].CityName.Should().Be(miamiCity.Name);
        result[0].Temperature.Should().Be(miamiCity.Weathers[0].Temperature);
        result[0].LastUpdate.Should().Be(miamiCity.Weathers[0].LastUpdate);

        result[1].CountryName.Should().Be(barcelonaCity.CountryName);
        result[1].CityName.Should().Be(barcelonaCity.Name);
        result[1].Temperature.Should().Be(barcelonaCity.Weathers[1].Temperature);
        result[1].LastUpdate.Should().Be(barcelonaCity.Weathers[1].LastUpdate);

        result[2].CountryName.Should().Be(barcelonaCity.CountryName);
        result[2].CityName.Should().Be(barcelonaCity.Name);
        result[2].Temperature.Should().Be(barcelonaCity.Weathers[2].Temperature);
        result[2].LastUpdate.Should().Be(barcelonaCity.Weathers[2].LastUpdate);
    }

    [Test]
    public async Task GetHighestWindSpeedModels_ForLastUpdates_ShouldGet()
    {
        // Arrange
        var now = DateTime.Now;
        _dateTimeServiceMock
            .Setup(x => x.UtcNow)
            .Returns(now);

        var miamiCity = new City
        {
            CountryName = "USA",
            Name = "Miami"
        };

        var miamiCityWeathers = new List<CityWeather>
        {
            new CityWeather { WindSpeed = 5, LastUpdate = now.AddMinutes(-1), City = miamiCity },
            new CityWeather { WindSpeed = 5, LastUpdate = now.AddMinutes(-2), City = miamiCity },
            new CityWeather { WindSpeed = 5, LastUpdate = now.AddMinutes(-3), City = miamiCity },
        };

        miamiCity.Weathers = miamiCityWeathers;

        var barcelonaCity = new City
        {
            CountryName = "Spain",
            Name = "Barcelona",

        };

        var barcelonaCityWeathers = new List<CityWeather>
        {
            new CityWeather { WindSpeed = 6, LastUpdate = now.AddMinutes(-1), City = barcelonaCity },
            new CityWeather { WindSpeed = 4, LastUpdate = now.AddMinutes(-2), City = barcelonaCity },
            new CityWeather { WindSpeed = 3, LastUpdate = now.AddMinutes(-3), City = barcelonaCity },
        };

        barcelonaCity.Weathers = barcelonaCityWeathers;

        var cities = new List<City>
        {
            miamiCity,
            barcelonaCity
        };

        cities[0].Weathers[0].City = cities[0];

        _cityRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(cities);

        // Act
        var result = await _cityWeatherService.GetHighestWindSpeedModels();

        // Assert
        result.Should().HaveCount(3);

        result[0].CountryName.Should().Be(barcelonaCity.CountryName);
        result[0].CityName.Should().Be(barcelonaCity.Name);
        result[0].HighestWind.Should().Be(barcelonaCity.Weathers[0].WindSpeed);
        result[0].LastUpdate.Should().Be(barcelonaCity.Weathers[0].LastUpdate);

        result[1].CountryName.Should().Be(miamiCity.CountryName);
        result[1].CityName.Should().Be(miamiCity.Name);
        result[1].HighestWind.Should().Be(miamiCity.Weathers[1].WindSpeed);
        result[1].LastUpdate.Should().Be(miamiCity.Weathers[1].LastUpdate);

        result[2].CountryName.Should().Be(miamiCity.CountryName);
        result[2].CityName.Should().Be(miamiCity.Name);
        result[2].HighestWind.Should().Be(miamiCity.Weathers[2].WindSpeed);
        result[2].LastUpdate.Should().Be(miamiCity.Weathers[2].LastUpdate);
    }
}
