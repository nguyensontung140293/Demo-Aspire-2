using AspireApp.ApiService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspireApp.ApiService.Features.WeatherForecasts.GetById;

public class GetWeatherForecastByIdHandler(WeatherDbContext db)
    : IRequestHandler<GetWeatherForecastByIdQuery, WeatherForecastResponse?>
{
    public async Task<WeatherForecastResponse?> Handle(
        GetWeatherForecastByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await db.WeatherForecasts
            .AsNoTracking()
            .Where(e => e.Id == request.Id)
            .Select(e => new WeatherForecastResponse(
                e.Id,
                e.Date,
                e.TemperatureC,
                32 + (int)(e.TemperatureC / 0.5556),
                e.Summary))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
