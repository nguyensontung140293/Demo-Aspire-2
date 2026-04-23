using Microsoft.EntityFrameworkCore;
using Weather.Domain.Entities;

namespace Weather.Application.Common.Interfaces;

public interface IWeatherWriteDbContext
{
    DbSet<WeatherForecast> WeatherForecasts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
