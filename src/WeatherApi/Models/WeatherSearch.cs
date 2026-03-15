namespace WeatherApi.Models;

public class WeatherSearch
{
    public int Id { get; set; }
    public string City { get; set; } = string.Empty;
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public DateTime SearchedAt { get; set; } = DateTime.UtcNow;
}