namespace Weather.Application.Features.WeatherForecasts;

public record WeatherForecastResponse(
    Guid Id,
    DateOnly Date,
    int TemperatureC,
    int TemperatureF,
    string? Summary);
