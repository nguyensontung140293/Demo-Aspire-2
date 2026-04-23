using MediatR;
using Microsoft.AspNetCore.Mvc;
using Weather.Application.Features.WeatherForecasts;
using Weather.Application.Features.WeatherForecasts.Create;
using Weather.Application.Features.WeatherForecasts.Delete;
using Weather.Application.Features.WeatherForecasts.GetAll;
using Weather.Application.Features.WeatherForecasts.GetById;
using Weather.Application.Features.WeatherForecasts.Update;

namespace Weather.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastsController(ISender sender) : ControllerBase
{
    /// <summary>Lấy danh sách tất cả dự báo thời tiết.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<WeatherForecastResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllWeatherForecastsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Lấy dự báo thời tiết theo ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<WeatherForecastResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetWeatherForecastByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Tạo mới một dự báo thời tiết.</summary>
    [HttpPost]
    [ProducesResponseType<WeatherForecastResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateWeatherForecastCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Cập nhật dự báo thời tiết theo ID.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<WeatherForecastResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateWeatherForecastRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateWeatherForecastCommand(id, request.Date, request.TemperatureC, request.Summary);
        var result = await sender.Send(command, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Xóa dự báo thời tiết theo ID.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await sender.Send(new DeleteWeatherForecastCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
