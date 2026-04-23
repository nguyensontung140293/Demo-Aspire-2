using MediatR;

namespace AspireApp.ApiService.Features.WeatherForecasts.GetById;

public record GetWeatherForecastByIdQuery(Guid Id) : IRequest<WeatherForecastResponse?>;
