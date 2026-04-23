using AspireApp.ApiService.Domain;
using AspireApp.ApiService.Infrastructure;
using MediatR;

namespace AspireApp.ApiService.Features.WeatherForecasts.Create;

public class CreateWeatherForecastHandler(WeatherDbContext db)
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

        return ToResponse(entity);
    }

    private static WeatherForecastResponse ToResponse(WeatherForecast e) =>
        new(e.Id, e.Date, e.TemperatureC, 32 + (int)(e.TemperatureC / 0.5556), e.Summary);
}
