using MediatR;

namespace AspireApp.ApiService.Features.WeatherForecasts.Update;

public record UpdateWeatherForecastCommand(
    Guid Id,
    DateOnly Date,
    int TemperatureC,
    string? Summary) : IRequest<WeatherForecastResponse?>;
