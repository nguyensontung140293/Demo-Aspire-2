using AspireApp.ApiService.Infrastructure;
using MediatR;

namespace AspireApp.ApiService.Features.WeatherForecasts.Update;

public class UpdateWeatherForecastHandler(WeatherDbContext db)
    : IRequestHandler<UpdateWeatherForecastCommand, WeatherForecastResponse?>
{
    public async Task<WeatherForecastResponse?> Handle(
        UpdateWeatherForecastCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await db.WeatherForecasts.FindAsync([request.Id], cancellationToken);
        if (entity is null)
            return null;

        entity.Date = request.Date;
        entity.TemperatureC = request.TemperatureC;
        entity.Summary = request.Summary;

        await db.SaveChangesAsync(cancellationToken);

        return new WeatherForecastResponse(
            entity.Id,
            entity.Date,
            entity.TemperatureC,
            32 + (int)(entity.TemperatureC / 0.5556),
            entity.Summary);
    }
}
