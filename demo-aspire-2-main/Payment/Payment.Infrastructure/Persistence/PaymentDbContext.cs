using Microsoft.EntityFrameworkCore;
using Payment.Application.Common.Interfaces;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.Persistence;

public class PaymentDbContext(DbContextOptions<PaymentDbContext> options)
    : DbContext(options), IPaymentWriteDbContext
{
    public DbSet<Domain.Entities.Payment> Payments => Set<Domain.Entities.Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Payment>(entity =>
        {
            entity.HasKey(payment => payment.Id);
            entity.Property(payment => payment.OrderId).HasMaxLength(100).IsRequired();
            entity.Property(payment => payment.Amount).HasPrecision(18, 2);
            entity.Property(payment => payment.Currency).HasMaxLength(3).IsRequired();
            entity.Property(payment => payment.Description).HasMaxLength(500);
            entity.Property(payment => payment.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.HasIndex(payment => payment.OrderId);
        });
    }
}
