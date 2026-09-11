using MediatR;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasana.API.Contracts.LoanRequests;
using QarzAlHasana.Application.Features.LoanRequests.Commands.ApproveLoanRequest;
using QarzAlHasana.Application.Features.LoanRequests.Commands.CreateLoanRequest;
using QarzAlHasana.Application.Features.LoanRequests.Commands.PayInstallment;
using QarzAlHasana.Application.Features.LoanRequests.Commands.RejectLoanRequest;
using QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanInstallments;
using QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanRequestById;
using QarzAlHasana.Application.Features.LoanRequests.Queries.GetMemberLoanRequests;
using QarzAlHasana.Application.Features.LoanRequests.Queries.GetPendingLoanRequests;

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

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
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
    
    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(
        Guid id,
        [FromBody] RejectLoanRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RejectLoanRequestCommand(id, request.RejectionReason);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:guid}/installments/{installmentId:guid}/pay")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PayInstallment(
        [FromRoute] Guid id,
        [FromRoute] Guid installmentId,
        [FromBody] PayInstallmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new PayInstallmentCommand(
            id,
            installmentId,
            request.AmountPaid);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
    
    [HttpGet("{id:guid}/installments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInstallments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetLoanInstallmentsQuery(id),
            cancellationToken);

        return Ok(result);
    }
    
    /// <summary>Liste darkhast-haye vam-e dar entezar-e barresi. Vizhe-ye Admin.</summary>
    [HttpGet("pending")]
    [ProducesResponseType(typeof(List<PendingLoanRequestDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PendingLoanRequestDto>>> GetPending(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPendingLoanRequestsQuery(), cancellationToken);

        return Ok(result);
    }
    /// <summary>Jozeiyât-e yek darkhâst-e vâm hamrâh bâ ozv va zâmen-hâ.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LoanRequestDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoanRequestDetailDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetLoanRequestByIdQuery(id), cancellationToken);

        return Ok(result);
    }
}