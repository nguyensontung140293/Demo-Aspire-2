using Microsoft.EntityFrameworkCore;

namespace Payment.Application.Common.Interfaces;

public interface IPaymentReadDbContext
{
    DbSet<Domain.Entities.Payment> Payments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
