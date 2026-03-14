using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherApi.Controllers;
using WeatherApi.Models;

namespace WeatherApi.Tests;

public class WeatherControllerTests
{
    private readonly WeatherController _controller;

    public WeatherControllerTests()
    {
        var logger = new Mock<ILogger<WeatherController>>();
        _controller = new WeatherController(logger.Object);
    }

    [Fact]
    public void Get_ReturnsExactlyFiveForecasts()
    {
        var result = _controller.Get();
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public void GetByCity_WithValidCity_ReturnsForecast()
    {
        var result = _controller.GetByCity("London");
        var ok = Assert.IsType<ActionResult<WeatherForecast>>(result);
        Assert.NotNull(ok.Value);
        Assert.Contains("London", ok.Value!.Summary);
    }

    [Fact]
    public void GetByCity_WithEmptyCity_ReturnsBadRequest()
    {
        var result = _controller.GetByCity("");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void GetIndia_ReturnsExactlyFiveCities()
    {
        var result = _controller.GetIndia();
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public void GetIndia_AllTemperaturesAreRealistic()
    {
        var result = _controller.GetIndia();
        Assert.All(result, f => Assert.InRange(f.TemperatureC, 20, 45));
    }
}