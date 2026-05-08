using BuildingBlocks.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payment.Application.Common.Interfaces;
using Payment.Infrastructure.Constants;
using Payment.Infrastructure.Persistence;

namespace Payment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PaymentDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("paymentdb"),
                npgsql => npgsql.MigrationsAssembly(typeof(PaymentDbContext).Assembly.FullName)
                                .MigrationsHistoryTable("__EFMigrationsHistory")));

        services.AddScoped<IPaymentWriteDbContext>(sp =>
            sp.GetRequiredService<PaymentDbContext>());

        services.AddDbContext<PaymentReadDbContext>(options =>
            options.UseNpgsql(
                    configuration.GetConnectionString("paymentdb-read"),
                    npgsql => npgsql.MigrationsAssembly(typeof(PaymentReadDbContext).Assembly.FullName)
                                    .MigrationsHistoryTable("__EFMigrationsHistory"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IPaymentReadDbContext>(sp =>
            sp.GetRequiredService<PaymentReadDbContext>());

        return services;
    }

    public static IHostApplicationBuilder AddInfrastructureCaching(
        this IHostApplicationBuilder builder)
    {
        builder.AddCustomCaching(redisConnectionStringName: AspireResources.Redis);
        return builder;
    }
}
