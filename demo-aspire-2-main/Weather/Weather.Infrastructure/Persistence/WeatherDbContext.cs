using Microsoft.EntityFrameworkCore;
using Weather.Application.Common.Interfaces;
using Weather.Domain.Entities;

namespace Weather.Infrastructure.Persistence;

public class WeatherDbContext(DbContextOptions<WeatherDbContext> options)
    : DbContext(options), IWeatherWriteDbContext
{
    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Summary).HasMaxLength(200);
        });
    }
}
