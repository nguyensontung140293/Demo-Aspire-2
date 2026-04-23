using Weather.Domain.Entities;

namespace Weather.Application.Features.WeatherForecasts;

public static class WeatherForecastMappings
{
    public static WeatherForecastResponse ToResponse(this WeatherForecast e) =>
        new(e.Id, e.Date, e.TemperatureC, 32 + (int)(e.TemperatureC / 0.5556), e.Summary);
}
