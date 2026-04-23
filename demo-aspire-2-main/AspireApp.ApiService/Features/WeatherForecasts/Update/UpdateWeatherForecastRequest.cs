namespace AspireApp.ApiService.Features.WeatherForecasts.Update;

public record UpdateWeatherForecastRequest(DateOnly Date, int TemperatureC, string? Summary);
