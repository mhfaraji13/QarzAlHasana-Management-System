using MediatR;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasana.Application.Features.LoanRequests.Commands.ApproveLoanRequest;
using QarzAlHasana.Application.Features.LoanRequests.Commands.CreateLoanRequest;
using QarzAlHasana.Application.Features.LoanRequests.Queries.GetMemberLoanRequests;

namespace QarzAlHasana.Api.Controllers;

[ApiController]
[Route("api/loan-requests")]
public sealed class LoanRequestsController : ControllerBase
{
    private readonly ISender _sender;

    public LoanRequestsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateLoanRequestCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);
        return Ok(id);
    }

    [HttpGet("member/{memberId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMember(
        [FromRoute] Guid memberId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetMemberLoanRequestsQuery(memberId),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Approve(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new ApproveLoanRequestCommand(id), cancellationToken);
        return NoContent();
    }
}