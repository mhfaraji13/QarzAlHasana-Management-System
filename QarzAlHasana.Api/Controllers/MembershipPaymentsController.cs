using MediatR;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasana.API.Contracts.MembershipPayments;
using QarzAlHasana.Application.Features.MembershipPayments.Commands.ApproveMembershipPayment;
using QarzAlHasana.Application.Features.MembershipPayments.Commands.CreateMembershipPayment;

namespace QarzAlHasana.Api.Controllers;

[ApiController]
[Route("api/membership-payments")]
public sealed class MembershipPaymentsController : ControllerBase
{
    private readonly ISender _sender;

    public MembershipPaymentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("member/{memberId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid memberId,
        [FromBody] CreateMembershipPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateMembershipPaymentCommand(
            memberId,
            request.Type,
            request.ForMonth,
            request.ForYear,
            request.ReceiptImageUrl);

        var id = await _sender.Send(command, cancellationToken);

        return Ok(id);
    }
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
}