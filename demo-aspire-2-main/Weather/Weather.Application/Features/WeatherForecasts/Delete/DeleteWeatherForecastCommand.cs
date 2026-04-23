using MediatR;

namespace Weather.Application.Features.WeatherForecasts.Delete;

public record DeleteWeatherForecastCommand(Guid Id) : IRequest<bool>;
