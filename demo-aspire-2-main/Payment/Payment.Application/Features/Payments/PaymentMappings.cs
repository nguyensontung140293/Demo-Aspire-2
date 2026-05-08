namespace Payment.Application.Features.Payments;

public static class PaymentMappings
{
    public static PaymentResponse ToResponse(this Domain.Entities.Payment payment) =>
        new(
            payment.Id,
            payment.OrderId,
            payment.Amount,
            payment.Currency,
            payment.Description,
            payment.Status,
            payment.CreatedAt,
            payment.CapturedAt,
            payment.RefundedAt);
}
