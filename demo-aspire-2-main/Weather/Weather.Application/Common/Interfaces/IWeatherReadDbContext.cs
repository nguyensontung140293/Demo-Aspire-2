using Microsoft.EntityFrameworkCore;
using Weather.Domain.Entities;

namespace Weather.Application.Common.Interfaces;

public interface IWeatherReadDbContext
{
    DbSet<WeatherForecast> WeatherForecasts { get; }
}
