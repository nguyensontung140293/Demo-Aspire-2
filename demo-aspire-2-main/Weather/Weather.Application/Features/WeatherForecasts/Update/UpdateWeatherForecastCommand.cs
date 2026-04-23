using MediatR;

namespace Weather.Application.Features.WeatherForecasts.Update;

public record UpdateWeatherForecastCommand(
    Guid Id,
    DateOnly Date,
    int TemperatureC,
    string? Summary) : IRequest<WeatherForecastResponse?>;

public record UpdateWeatherForecastRequest(DateOnly Date, int TemperatureC, string? Summary);
