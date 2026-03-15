using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherApi.Controllers;
using WeatherApi.Data;
using WeatherApi.Models;

namespace WeatherApi.Tests;

public class WeatherControllerTests : IDisposable
{
    private readonly WeatherController _controller;
    private readonly WeatherContext _context;

    public WeatherControllerTests()
    {
        var options = new DbContextOptionsBuilder<WeatherContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new WeatherContext(options);
        var logger = new Mock<ILogger<WeatherController>>();
        _controller = new WeatherController(logger.Object, _context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void Get_ReturnsExactlyFiveForecasts()
    {
        var result = _controller.Get();
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task GetByCity_WithValidCity_ReturnsForecast()
    {
        var result = await _controller.GetByCity("London");
        var ok = Assert.IsType<ActionResult<WeatherForecast>>(result);
        Assert.NotNull(ok.Value);
        Assert.Contains("London", ok.Value!.Summary);
    }

    [Fact]
    public async Task GetByCity_WithEmptyCity_ReturnsBadRequest()
    {
        var result = await _controller.GetByCity("");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetIndia_ReturnsExactlyFiveCities()
    {
        var result = await _controller.GetIndia();
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task GetIndia_AllTemperaturesAreRealistic()
    {
        var result = await _controller.GetIndia();
        Assert.All(result, f => Assert.InRange(f.TemperatureC, 20, 45));
    }

    [Fact]
    public async Task GetByCity_SavesSearchToDatabase()
    {
        await _controller.GetByCity("Mumbai");
        var history = await _context.WeatherSearches.ToListAsync();
        Assert.Single(history);
        Assert.Equal("Mumbai", history[0].City);
    }
}