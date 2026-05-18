using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Caching;

public static class DependencyInjection
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services, string connectionString)
    {
        services.AddStackExchangeRedisCache(options =>
            options.Configuration = connectionString);

        services.AddSingleton<ICacheService, RedisCacheService>();
        return services;
    }

    public static IHostApplicationBuilder AddCustomCaching(
        this IHostApplicationBuilder builder,
        string redisConnectionStringName)
    {
        var connectionString = builder.Configuration.GetConnectionString(redisConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{redisConnectionStringName}' not found.");

        builder.Services.AddRedisCache(connectionString);
        return builder;
    }
}
