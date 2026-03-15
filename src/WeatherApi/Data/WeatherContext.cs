using Microsoft.EntityFrameworkCore;
using WeatherApi.Models;

namespace WeatherApi.Data;

public class WeatherContext : DbContext
{
    public WeatherContext(DbContextOptions<WeatherContext> options) : base(options) { }

    public DbSet<WeatherSearch> WeatherSearches => Set<WeatherSearch>();
}