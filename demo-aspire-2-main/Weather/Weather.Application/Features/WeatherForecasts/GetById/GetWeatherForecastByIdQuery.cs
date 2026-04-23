using MediatR;

namespace Weather.Application.Features.WeatherForecasts.GetById;

public record GetWeatherForecastByIdQuery(Guid Id) : IRequest<WeatherForecastResponse?>;
