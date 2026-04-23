using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Weather.Domain.Entities;

namespace Weather.Infrastructure.Persistence;

public class WeatherDataSeeder(
    WeatherDbContext writeDb,
    WeatherReadDbContext readDb,
    ILogger<WeatherDataSeeder> logger)
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await writeDb.WeatherForecasts.AnyAsync(cancellationToken))
        {
            logger.LogInformation("WeatherForecasts already seeded. Skipping.");
            return;
        }

        var forecasts = new List<WeatherForecast>
        {
            new() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today),           TemperatureC = -5,  Summary = Summaries[0] },
            new() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), TemperatureC = 2,   Summary = Summaries[1] },
            new() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today.AddDays(2)), TemperatureC = 8,   Summary = Summaries[2] },
            new() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today.AddDays(3)), TemperatureC = 13,  Summary = Summaries[3] },
            new() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today.AddDays(4)), TemperatureC = 17,  Summary = Summaries[4] },
            new() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today.AddDays(5)), TemperatureC = 22,  Summary = Summaries[5] },
            new() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today.AddDays(6)), TemperatureC = 27,  Summary = Summaries[6] },
            new() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today.AddDays(7)), TemperatureC = 32,  Summary = Summaries[7] },
            new() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today.AddDays(8)), TemperatureC = 38,  Summary = Summaries[8] },
            new() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today.AddDays(9)), TemperatureC = 42,  Summary = Summaries[9] },
        };

        // Seed vào Write DB
        await writeDb.WeatherForecasts.AddRangeAsync(forecasts, cancellationToken);
        await writeDb.SaveChangesAsync(cancellationToken);

        // Sync dữ liệu sang Read DB
        await readDb.WeatherForecasts.AddRangeAsync(forecasts, cancellationToken);
        await readDb.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded {Count} WeatherForecasts to Write and Read DB successfully.", forecasts.Count);
    }
}
