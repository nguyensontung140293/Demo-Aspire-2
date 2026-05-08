using BuildingBlocks.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Common.Interfaces;

namespace Payment.Application.Features.Payments.GetById;

public class GetPaymentByIdHandler(IPaymentReadDbContext db, ICacheService cache)
    : IRequestHandler<GetPaymentByIdQuery, PaymentResponse?>
{
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(2);

    public async Task<PaymentResponse?> Handle(
        GetPaymentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"payments:{request.Id}";
        var cached = await cache.GetAsync<PaymentResponse>(cacheKey, cancellationToken);
        if (cached is not null)
            return cached;

        var payment = await db.Payments
            .AsNoTracking()
            .FirstOrDefaultAsync(payment => payment.Id == request.Id, cancellationToken);

        var result = payment?.ToResponse();
        if (result is not null)
            await cache.SetAsync(cacheKey, result, CacheExpiry, cancellationToken);

        return result;
    }
}
