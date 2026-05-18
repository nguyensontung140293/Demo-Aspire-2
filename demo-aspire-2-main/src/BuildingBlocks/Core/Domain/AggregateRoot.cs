using BuildingBlocks.Abstractions.CQRS;

namespace BuildingBlocks.Core.Domain;

/// <summary>Aggregate Root — quản lý domain events.</summary>
public abstract class AggregateRoot<TId> : BaseEntity<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}

/// <summary>Aggregate Root với Guid Id.</summary>
public abstract class AggregateRoot : AggregateRoot<Guid>
{
    protected AggregateRoot() => Id = Guid.NewGuid();
}
