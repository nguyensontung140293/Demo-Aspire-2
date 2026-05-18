namespace BuildingBlocks.Abstractions.CQRS;

/// <summary>Domain Event — xảy ra trong bounded context.</summary>
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}

/// <summary>Integration Event — giao tiếp giữa các microservices qua message bus.</summary>
public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
    string EventType { get; }
}
