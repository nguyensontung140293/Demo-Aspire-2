using MediatR;

namespace Payment.Application.Features.Payments.Refund;

public record RefundPaymentCommand(Guid Id) : IRequest<PaymentResponse?>;
