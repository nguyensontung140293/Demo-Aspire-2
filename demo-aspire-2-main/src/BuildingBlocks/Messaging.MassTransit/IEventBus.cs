using BuildingBlocks.Abstractions.CQRS;

namespace BuildingBlocks.Messaging.MassTransit;

/// <summary>Event bus interface — publish integration events.</summary>
public interface IEventBus
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent;
}
