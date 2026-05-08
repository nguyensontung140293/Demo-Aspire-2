using MediatR;

namespace Payment.Application.Features.Payments.Create;

public record CreatePaymentCommand(
    string OrderId,
    decimal Amount,
    string Currency,
    string? Description) : IRequest<PaymentResponse>;
