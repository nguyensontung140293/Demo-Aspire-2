using MediatR;

namespace AspireApp.ApiService.Features.WeatherForecasts.Create;

public record CreateWeatherForecastCommand(
    DateOnly Date,
    int TemperatureC,
    string? Summary) : IRequest<WeatherForecastResponse>;
