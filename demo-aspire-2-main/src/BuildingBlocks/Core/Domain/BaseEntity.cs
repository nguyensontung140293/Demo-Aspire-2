namespace BuildingBlocks.Core.Domain;

/// <summary>Base entity với Id và audit timestamps.</summary>
public abstract class BaseEntity<TId>
{
    public TId Id { get; protected set; } = default!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
}

/// <summary>Base entity với Guid Id.</summary>
public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity() => Id = Guid.NewGuid();
}
