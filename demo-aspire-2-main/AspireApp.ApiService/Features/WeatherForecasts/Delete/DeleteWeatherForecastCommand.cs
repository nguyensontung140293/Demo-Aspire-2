using MediatR;

namespace AspireApp.ApiService.Features.WeatherForecasts.Delete;

public record DeleteWeatherForecastCommand(Guid Id) : IRequest<bool>;
