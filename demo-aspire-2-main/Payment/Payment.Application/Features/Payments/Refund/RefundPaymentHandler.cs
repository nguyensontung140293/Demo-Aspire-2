using MediatR;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Common.Interfaces;
using Payment.Domain.Entities;

namespace Payment.Application.Features.Payments.Refund;

public class RefundPaymentHandler(IPaymentWriteDbContext writeDb, IPaymentReadDbContext readDb)
    : IRequestHandler<RefundPaymentCommand, PaymentResponse?>
{
    public async Task<PaymentResponse?> Handle(
        RefundPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await writeDb.Payments.FirstOrDefaultAsync(payment => payment.Id == request.Id, cancellationToken);
        if (payment is null)
            return null;

        if (payment.Status != PaymentStatus.Captured)
            throw new InvalidOperationException("Only captured payments can be refunded.");

        payment.Status = PaymentStatus.Refunded;
        payment.RefundedAt = DateTimeOffset.UtcNow;

        var projection = await readDb.Payments.AsTracking().FirstOrDefaultAsync(payment => payment.Id == request.Id, cancellationToken);
        if (projection is not null)
        {
            projection.Status = payment.Status;
            projection.RefundedAt = payment.RefundedAt;
        }

        await writeDb.SaveChangesAsync(cancellationToken);
        await readDb.SaveChangesAsync(cancellationToken);

        return payment.ToResponse();
    }
}
