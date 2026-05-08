using MediatR;
using Payment.Application.Common.Interfaces;

namespace Payment.Application.Features.Payments.Create;

public class CreatePaymentHandler(IPaymentWriteDbContext writeDb, IPaymentReadDbContext readDb)
    : IRequestHandler<CreatePaymentCommand, PaymentResponse>
{
    public async Task<PaymentResponse> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.OrderId))
            throw new ArgumentException("OrderId is required.", nameof(request.OrderId));

        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(request.Amount));

        var payment = new Domain.Entities.Payment
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId.Trim(),
            Amount = request.Amount,
            Currency = string.IsNullOrWhiteSpace(request.Currency) ? "USD" : request.Currency.Trim().ToUpperInvariant(),
            Description = request.Description,
            CreatedAt = DateTimeOffset.UtcNow
        };

        writeDb.Payments.Add(payment);
        readDb.Payments.Add(new Domain.Entities.Payment
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            Description = payment.Description,
            Status = payment.Status,
            CreatedAt = payment.CreatedAt
        });

        await writeDb.SaveChangesAsync(cancellationToken);
        await readDb.SaveChangesAsync(cancellationToken);

        return payment.ToResponse();
    }
}
