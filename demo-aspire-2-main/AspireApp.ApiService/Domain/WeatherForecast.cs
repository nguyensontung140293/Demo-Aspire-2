namespace AspireApp.ApiService.Domain;

public class WeatherForecast
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}
