using BuildingBlocks.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weather.Application.Common.Interfaces;
using Weather.Infrastructure.Constants;
using Weather.Infrastructure.Persistence;

namespace Weather.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Write DB
        services.AddDbContext<WeatherDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("weatherdb"),
                npgsql => npgsql.MigrationsAssembly(typeof(WeatherDbContext).Assembly.FullName)
                                .MigrationsHistoryTable("__EFMigrationsHistory")));

        services.AddScoped<IWeatherWriteDbContext>(sp =>
            sp.GetRequiredService<WeatherDbContext>());

        // Read DB
        services.AddDbContext<WeatherReadDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("weatherdb-read"),
                npgsql => npgsql.MigrationsAssembly(typeof(WeatherReadDbContext).Assembly.FullName)
                                .MigrationsHistoryTable("__EFMigrationsHistory"))
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IWeatherReadDbContext>(sp =>
            sp.GetRequiredService<WeatherReadDbContext>());

        services.AddScoped<WeatherDataSeeder>();

        return services;
    }

    // Caching — Redis (connection string from Aspire)
    public static IHostApplicationBuilder AddInfrastructureCaching(
        this IHostApplicationBuilder builder)
    {
        builder.AddCustomCaching(redisConnectionStringName: AspireResources.Redis);
        return builder;
    }
}
