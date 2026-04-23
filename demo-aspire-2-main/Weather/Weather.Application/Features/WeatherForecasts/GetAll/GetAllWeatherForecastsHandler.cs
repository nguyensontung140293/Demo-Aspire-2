using BuildingBlocks.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.Application.Common.Interfaces;

namespace Weather.Application.Features.WeatherForecasts.GetAll;

public class GetAllWeatherForecastsHandler(IWeatherReadDbContext db, ICacheService cache)
    : IRequestHandler<GetAllWeatherForecastsQuery, IEnumerable<WeatherForecastResponse>>
{
    private const string CacheKey = "weather:forecasts:all";
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5);

    public async Task<IEnumerable<WeatherForecastResponse>> Handle(
        GetAllWeatherForecastsQuery request,
        CancellationToken cancellationToken)
    {
        var cached = await cache.GetAsync<List<WeatherForecastResponse>>(CacheKey, cancellationToken);
        if (cached is not null)
            return cached;

        var result = await db.WeatherForecasts
            .AsNoTracking()
            .Select(e => e.ToResponse())
            .ToListAsync(cancellationToken);

        await cache.SetAsync(CacheKey, result, CacheExpiry, cancellationToken);

        return result;
    }
}
