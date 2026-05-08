using MediatR;

namespace Payment.Application.Features.Payments.Capture;

public record CapturePaymentCommand(Guid Id) : IRequest<PaymentResponse?>;
