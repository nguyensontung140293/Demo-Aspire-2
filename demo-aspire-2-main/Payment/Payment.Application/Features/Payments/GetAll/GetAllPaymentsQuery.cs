using MediatR;

namespace Payment.Application.Features.Payments.GetAll;

public record GetAllPaymentsQuery : IRequest<IEnumerable<PaymentResponse>>;
