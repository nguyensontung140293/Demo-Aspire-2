using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Features.Payments;
using Payment.Application.Features.Payments.Capture;
using Payment.Application.Features.Payments.Create;
using Payment.Application.Features.Payments.GetAll;
using Payment.Application.Features.Payments.GetById;
using Payment.Application.Features.Payments.Refund;

namespace Payment.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController(ISender sender) : ControllerBase
{
    /// <summary>Lấy danh sách tất cả giao dịch thanh toán.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<PaymentResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAllPaymentsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Lấy giao dịch thanh toán theo ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<PaymentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPaymentByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Tạo mới giao dịch thanh toán ở trạng thái Pending.</summary>
    [HttpPost]
    [ProducesResponseType<PaymentResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePaymentCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>Capture một giao dịch thanh toán Pending.</summary>
    [HttpPost("{id:guid}/capture")]
    [ProducesResponseType<PaymentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Capture(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new CapturePaymentCommand(id), cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>Refund một giao dịch thanh toán đã Capture.</summary>
    [HttpPost("{id:guid}/refund")]
    [ProducesResponseType<PaymentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Refund(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new RefundPaymentCommand(id), cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
