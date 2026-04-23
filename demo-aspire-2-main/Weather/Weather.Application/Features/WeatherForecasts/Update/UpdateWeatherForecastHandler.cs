using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.Application.Common.Interfaces;

namespace Weather.Application.Features.WeatherForecasts.Update;

public class UpdateWeatherForecastHandler(IWeatherWriteDbContext db)
    : IRequestHandler<UpdateWeatherForecastCommand, WeatherForecastResponse?>
{
    public async Task<WeatherForecastResponse?> Handle(
        UpdateWeatherForecastCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await db.WeatherForecasts
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return null;

        entity.Date = request.Date;
        entity.TemperatureC = request.TemperatureC;
        entity.Summary = request.Summary;

        await db.SaveChangesAsync(cancellationToken);

        return entity.ToResponse();
    }
}
