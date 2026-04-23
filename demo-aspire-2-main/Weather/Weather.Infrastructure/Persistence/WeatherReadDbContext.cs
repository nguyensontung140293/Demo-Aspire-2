using Microsoft.EntityFrameworkCore;
using Weather.Application.Common.Interfaces;
using Weather.Domain.Entities;

namespace Weather.Infrastructure.Persistence;

public class WeatherReadDbContext(DbContextOptions<WeatherReadDbContext> options)
    : DbContext(options), IWeatherReadDbContext
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}
