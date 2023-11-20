using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WeatherApp.Application.Services.Implementation;
using WeatherApp.Application.Services.Interfaces;
using WeatherApp.Core.Interfaces.Repositories;
using WeatherApp.Core.Models;

namespace WeatherApp.Application.Tests.Services;

public class CityServiceTests
{
    private IFixture _fixture;
    private Mock<ICityRepository> _cityRepositoryMock;
    private ICityService _cityService;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _cityRepositoryMock = _fixture.Freeze<Mock<ICityRepository>>();
        _cityService = _fixture.Create<CityService>();
    }

    [Test]
    public async Task GetLowestTemperatureModels_ForLastUpdates_ShouldGet()
    {
        // Arrange
        var miamiCity = new City
        {
            CountryName = "USA",
            Name = "Miami"
        };

        var barcelonaCity = new City
        {
            CountryName = "Spain",
            Name = "Barcelona",

        };

        var cities = new List<City>
        {
            miamiCity,
            barcelonaCity
        };

        _cityRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(cities);

        // Act
        var result = await _cityService.Get();

        // Assert
        result.Should().HaveCount(2);

        result[0].CountryName.Should().Be(miamiCity.CountryName);
        result[0].CityName.Should().Be(miamiCity.Name);

        result[1].CountryName.Should().Be(barcelonaCity.CountryName);
        result[1].CityName.Should().Be(barcelonaCity.Name);
    }
}
