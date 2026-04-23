using MediatR;

namespace Weather.Application.Features.WeatherForecasts.GetAll;

public record GetAllWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecastResponse>>;
