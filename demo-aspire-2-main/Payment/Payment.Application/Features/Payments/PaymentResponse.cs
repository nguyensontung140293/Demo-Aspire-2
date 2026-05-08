using Payment.Domain.Entities;

namespace Payment.Application.Features.Payments;

public record PaymentResponse(
    Guid Id,
    string OrderId,
    decimal Amount,
    string Currency,
    string? Description,
    PaymentStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? CapturedAt,
    DateTimeOffset? RefundedAt);
