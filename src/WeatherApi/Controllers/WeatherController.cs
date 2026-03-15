using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Data;
using WeatherApi.Models;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private static readonly string[] Summaries =
        ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot"];

    private readonly ILogger<WeatherController> _logger;
    private readonly WeatherContext _context;

    public WeatherController(ILogger<WeatherController> logger, WeatherContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("Fetching weather forecasts");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        });
    }

    [HttpGet("{city}")]
    public async Task<ActionResult<WeatherForecast>> GetByCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return BadRequest("City name is required.");

        var forecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = Random.Shared.Next(-10, 40),
            Summary = $"{Summaries[Random.Shared.Next(Summaries.Length)]} in {city}"
        };

        _context.WeatherSearches.Add(new WeatherSearch
        {
            City = city,
            TemperatureC = forecast.TemperatureC,
            Summary = forecast.Summary,
            SearchedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        return forecast;
    }

    [HttpGet("india")]
    public async Task<IEnumerable<WeatherForecast>> GetIndia()
    {
        _logger.LogInformation("Fetching weather forecasts for India");
        string[] indianCities = ["Mumbai", "Delhi", "Bangalore", "Chennai", "Hyderabad"];

        var forecasts = indianCities.Select(city => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = Random.Shared.Next(20, 45),
            Summary = $"{Summaries[Random.Shared.Next(Summaries.Length)]} in {city}"
        }).ToList();

        foreach (var (city, forecast) in indianCities.Zip(forecasts))
        {
            _context.WeatherSearches.Add(new WeatherSearch
            {
                City = city,
                TemperatureC = forecast.TemperatureC,
                Summary = forecast.Summary,
                SearchedAt = DateTime.UtcNow
            });
        }
        await _context.SaveChangesAsync();

        return forecasts;
    }

    [HttpGet("history")]
    public async Task<IEnumerable<WeatherSearch>> GetHistory()
    {
        _logger.LogInformation("Fetching search history");
        return await _context.WeatherSearches
            .OrderByDescending(w => w.SearchedAt)
            .Take(20)
            .ToListAsync();
    }
}