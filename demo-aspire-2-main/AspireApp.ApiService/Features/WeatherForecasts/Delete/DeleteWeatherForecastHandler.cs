using AspireApp.ApiService.Infrastructure;
using MediatR;

namespace AspireApp.ApiService.Features.WeatherForecasts.Delete;

public class DeleteWeatherForecastHandler(WeatherDbContext db)
    : IRequestHandler<DeleteWeatherForecastCommand, bool>
{
    public async Task<bool> Handle(
        DeleteWeatherForecastCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await db.WeatherForecasts.FindAsync([request.Id], cancellationToken);
        if (entity is null)
            return false;

        db.WeatherForecasts.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
