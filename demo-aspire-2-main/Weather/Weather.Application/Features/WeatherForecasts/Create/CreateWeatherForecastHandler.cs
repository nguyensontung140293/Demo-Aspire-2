using MediatR;
using Weather.Application.Common.Interfaces;
using Weather.Domain.Entities;

namespace Weather.Application.Features.WeatherForecasts.Create;

public class CreateWeatherForecastHandler(IWeatherWriteDbContext db)
    : IRequestHandler<CreateWeatherForecastCommand, WeatherForecastResponse>
{
    public async Task<WeatherForecastResponse> Handle(
        CreateWeatherForecastCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new WeatherForecast
        {
            Id = Guid.NewGuid(),
            Date = request.Date,
            TemperatureC = request.TemperatureC,
            Summary = request.Summary
        };

        db.WeatherForecasts.Add(entity);
        await db.SaveChangesAsync(cancellationToken);

        return entity.ToResponse();
    }
}
