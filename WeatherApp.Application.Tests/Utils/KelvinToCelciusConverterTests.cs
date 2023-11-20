using FluentAssertions;
using NUnit.Framework;
using WeatherApp.Application.Utils.Implementation;
using WeatherApp.Application.Utils.Interfaces;

namespace WeatherApp.Application.Tests.Utils;

public class KelvinToCelciusConverterTests
{
    private IKelvinToCelciusConverter _kelvinToCelciusConverter;

    [SetUp]
    public void SetUp()
    {
        _kelvinToCelciusConverter = new KelvinToCelciusConverter();
    }

    [Test]
    public void Convert_ForKelvinToCelciusConversion_ShouldConvert()
    {
        // Arrange
        var kelvinValue = 300.45;

        // Act
        var result = _kelvinToCelciusConverter.Convert(kelvinValue);

        // Assert
        result.Should().Be(27.3);
    }
}
