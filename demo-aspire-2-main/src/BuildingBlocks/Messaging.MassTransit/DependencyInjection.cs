using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMqMessaging(
        this IServiceCollection services,
        string host,
        string username,
        string password,
        Assembly? consumersAssembly = null)
    {
        services.AddMassTransit(x =>
        {
            if (consumersAssembly is not null)
                x.AddConsumers(consumersAssembly);

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(host, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                cfg.ConfigureEndpoints(ctx);
            });
        });

        services.AddScoped<IEventBus, MassTransitEventBus>();
        return services;
    }
}
