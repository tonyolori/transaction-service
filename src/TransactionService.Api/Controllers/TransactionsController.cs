using Microsoft.AspNetCore.Mvc;
using MediatR;
using TransactionService.Application.Commands;
using TransactionService.Application.Queries;

[ApiController]
[Route("api/v1/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    public TransactionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("/{id}")]
    public async Task<IActionResult> GetTransaction([FromRoute] Guid id)
    {
        var q = new GetTransactionQuery(id);
        var res = await _mediator.Send(q);
        return res == null ? NotFound() : Ok(res);
    }

    [HttpPost("/{id}/receipt/share")]
    public async Task<IActionResult> CreateShareableReceipt([FromRoute] Guid id, [FromBody] CreateShareRequest req)
    {
        var cmd = new CreateShareableLinkCommand(id, req.ExpiresInSeconds);
        var link = await _mediator.Send(cmd);
        return Ok(new { link });
    }
}

public record CreateShareRequest(int ExpiresInSeconds);
