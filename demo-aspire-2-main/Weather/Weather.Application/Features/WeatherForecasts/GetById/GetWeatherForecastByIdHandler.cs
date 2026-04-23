using BuildingBlocks.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.Application.Common.Interfaces;

namespace Weather.Application.Features.WeatherForecasts.GetById;

public class GetWeatherForecastByIdHandler(IWeatherReadDbContext db, ICacheService cache)
    : IRequestHandler<GetWeatherForecastByIdQuery, WeatherForecastResponse?>
{
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5);

    public async Task<WeatherForecastResponse?> Handle(
        GetWeatherForecastByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"weather:forecasts:{request.Id}";

        var cached = await cache.GetAsync<WeatherForecastResponse>(cacheKey, cancellationToken);
        if (cached is not null)
            return cached;

        var entity = await db.WeatherForecasts
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        var result = entity?.ToResponse();

        if (result is not null)
            await cache.SetAsync(cacheKey, result, CacheExpiry, cancellationToken);

        return result;
    }
}
