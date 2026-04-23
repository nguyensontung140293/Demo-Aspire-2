using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.Application.Common.Interfaces;

namespace Weather.Application.Features.WeatherForecasts.Delete;

public class DeleteWeatherForecastHandler(IWeatherWriteDbContext db)
    : IRequestHandler<DeleteWeatherForecastCommand, bool>
{
    public async Task<bool> Handle(
        DeleteWeatherForecastCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await db.WeatherForecasts
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return false;

        db.WeatherForecasts.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
