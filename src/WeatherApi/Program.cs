using Microsoft.EntityFrameworkCore;
using Prometheus;
using WeatherApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Weather API", Version = "v1" });
});

builder.Services.AddHealthChecks()
    .AddDbContextCheck<WeatherContext>();

builder.Services.AddDbContext<WeatherContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpMetrics();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapMetrics();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WeatherContext>();
    db.Database.MigrateAsync().Wait();
}

app.Run();