using AspireApp.ApiService.Domain;
using AspireApp.ApiService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspireApp.ApiService.Features.WeatherForecasts.GetAll;

public class GetAllWeatherForecastsHandler(WeatherDbContext db)
    : IRequestHandler<GetAllWeatherForecastsQuery, IEnumerable<WeatherForecastResponse>>
{
    public async Task<IEnumerable<WeatherForecastResponse>> Handle(
        GetAllWeatherForecastsQuery request,
        CancellationToken cancellationToken)
    {
        return await db.WeatherForecasts
            .AsNoTracking()
            .Select(e => new WeatherForecastResponse(
                e.Id,
                e.Date,
                e.TemperatureC,
                32 + (int)(e.TemperatureC / 0.5556),
                e.Summary))
            .ToListAsync(cancellationToken);
    }
}
