using BuildingBlocks.Abstractions.CQRS;
using BuildingBlocks.Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.EfCore;

/// <summary>
/// Base DbContext — tự động set audit timestamps và dispatch domain events.
/// </summary>
public abstract class BaseDbContext(
    DbContextOptions options,
    IPublisher? publisher = null) : DbContext(options)
{
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditTimestamps();
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync(cancellationToken);
        return result;
    }

    private void SetAuditTimestamps()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.SetUpdatedAt();
        }
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        if (publisher is null) return;

        var aggregates = ChangeTracker.Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var events = aggregates.SelectMany(a => a.DomainEvents).ToList();

        aggregates.ForEach(a => a.ClearDomainEvents());

        foreach (var @event in events)
            await publisher.Publish((INotification)@event, cancellationToken);
    }
}
