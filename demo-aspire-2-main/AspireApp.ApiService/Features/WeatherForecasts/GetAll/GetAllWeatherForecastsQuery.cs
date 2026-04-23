using MediatR;

namespace AspireApp.ApiService.Features.WeatherForecasts.GetAll;

public record GetAllWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecastResponse>>;
