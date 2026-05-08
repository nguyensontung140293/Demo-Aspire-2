using BuildingBlocks.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Common.Interfaces;

namespace Payment.Application.Features.Payments.GetAll;

public class GetAllPaymentsHandler(IPaymentReadDbContext db, ICacheService cache)
    : IRequestHandler<GetAllPaymentsQuery, IEnumerable<PaymentResponse>>
{
    private const string CacheKey = "payments:all";
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(2);

    public async Task<IEnumerable<PaymentResponse>> Handle(
        GetAllPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var cached = await cache.GetAsync<List<PaymentResponse>>(CacheKey, cancellationToken);
        if (cached is not null)
            return cached;

        var result = await db.Payments
            .AsNoTracking()
            .OrderByDescending(payment => payment.CreatedAt)
            .Select(payment => payment.ToResponse())
            .ToListAsync(cancellationToken);

        await cache.SetAsync(CacheKey, result, CacheExpiry, cancellationToken);

        return result;
    }
}
