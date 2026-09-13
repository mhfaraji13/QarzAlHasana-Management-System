using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasana.API.Contracts.MembershipPayments;
using QarzAlHasana.Application.Features.MembershipPayments.Commands.ApproveMembershipPayment;
using QarzAlHasana.Application.Features.MembershipPayments.Commands.CreateMembershipPayment;
using QarzAlHasana.Application.Features.MembershipPayments.Commands.RejectMembershipPayment;
using QarzAlHasana.Application.Features.MembershipPayments.Queries.GetMemberPayments;
using QarzAlHasana.Application.Features.MembershipPayments.Queries.GetPendingPayments;

namespace QarzAlHasana.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/membership-payments")]
public sealed class MembershipPaymentsController : ControllerBase
{
    private readonly ISender _sender;

    public MembershipPaymentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateMembershipPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateMembershipPaymentCommand(
            request.Type,
            request.ForMonth,
            request.ForYear,
            request.ReceiptImageUrl);

        var id = await _sender.Send(command, cancellationToken);

        return Ok(id);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Approve(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new ApproveMembershipPaymentCommand(id), cancellationToken);

        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Reject(
        [FromRoute] Guid id,
        [FromBody] RejectMembershipPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RejectMembershipPaymentCommand(id, request.RejectionReason);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
    
    [HttpGet("me")]
    [ProducesResponseType(typeof(List<MemberPaymentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MemberPaymentDto>>> GetMine(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMemberPaymentsQuery(), cancellationToken);

        return Ok(result);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("pending")]
    [ProducesResponseType(typeof(List<PendingPaymentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PendingPaymentDto>>> GetPending(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPendingPaymentsQuery(), cancellationToken);

        return Ok(result);
    }
}