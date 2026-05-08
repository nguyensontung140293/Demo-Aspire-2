using MediatR;

namespace Payment.Application.Features.Payments.GetById;

public record GetPaymentByIdQuery(Guid Id) : IRequest<PaymentResponse?>;
