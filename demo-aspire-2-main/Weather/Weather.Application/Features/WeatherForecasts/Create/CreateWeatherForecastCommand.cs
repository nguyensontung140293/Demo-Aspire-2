using MediatR;

namespace Weather.Application.Features.WeatherForecasts.Create;

public record CreateWeatherForecastCommand(
    DateOnly Date,
    int TemperatureC,
    string? Summary) : IRequest<WeatherForecastResponse>;
