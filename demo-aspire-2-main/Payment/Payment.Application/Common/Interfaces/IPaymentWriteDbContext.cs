using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;

namespace Payment.Application.Common.Interfaces;

public interface IPaymentWriteDbContext
{
    DbSet<Domain.Entities.Payment> Payments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
