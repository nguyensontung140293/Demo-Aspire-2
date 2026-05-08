using MediatR;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Common.Interfaces;
using Payment.Domain.Entities;

namespace Payment.Application.Features.Payments.Capture;

public class CapturePaymentHandler(IPaymentWriteDbContext writeDb, IPaymentReadDbContext readDb)
    : IRequestHandler<CapturePaymentCommand, PaymentResponse?>
{
    public async Task<PaymentResponse?> Handle(
        CapturePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await writeDb.Payments.FirstOrDefaultAsync(payment => payment.Id == request.Id, cancellationToken);
        if (payment is null)
            return null;

        if (payment.Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be captured.");

        payment.Status = PaymentStatus.Captured;
        payment.CapturedAt = DateTimeOffset.UtcNow;

        var projection = await readDb.Payments.AsTracking().FirstOrDefaultAsync(payment => payment.Id == request.Id, cancellationToken);
        if (projection is not null)
        {
            projection.Status = payment.Status;
            projection.CapturedAt = payment.CapturedAt;
        }

        await writeDb.SaveChangesAsync(cancellationToken);
        await readDb.SaveChangesAsync(cancellationToken);

        return payment.ToResponse();
    }
}
