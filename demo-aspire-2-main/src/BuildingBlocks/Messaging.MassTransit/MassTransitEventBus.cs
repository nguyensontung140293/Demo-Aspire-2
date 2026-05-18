using BuildingBlocks.Abstractions.CQRS;
using MassTransit;

namespace BuildingBlocks.Messaging.MassTransit;

public sealed class MassTransitEventBus(IBus bus) : IEventBus
{
    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent =>
        bus.Publish(@event, cancellationToken);
}
