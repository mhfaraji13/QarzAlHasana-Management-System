using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasana.API.Contracts.SupportTickets;
using QarzAlHasana.Application.Features.SupportTickets.Commands.AddSupportMessage;
using QarzAlHasana.Application.Features.SupportTickets.Commands.CloseSupportTicket;
using QarzAlHasana.Application.Features.SupportTickets.Commands.CreateSupportTicket;
using QarzAlHasana.Application.Features.SupportTickets.Queries.GetMemberTickets;
using QarzAlHasana.Application.Features.SupportTickets.Queries.GetOpenTickets;
using QarzAlHasana.Application.Features.SupportTickets.Queries.GetTicketById;

namespace QarzAlHasana.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/support-tickets")]
public sealed class SupportTicketsController : ControllerBase
{
    private readonly ISender _sender;

    public SupportTicketsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("member/{memberId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid memberId,
        [FromBody] CreateSupportTicketRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateSupportTicketCommand(
            memberId,
            request.Subject,
            request.Content);

        var id = await _sender.Send(command, cancellationToken);

        return Ok(id);
    }

    [HttpPost("{id:guid}/messages")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddMessage(
        [FromRoute] Guid id,
        [FromBody] AddSupportMessageRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddSupportMessageCommand(
            id,
            request.Content,
            request.SenderType);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Close(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new CloseSupportTicketCommand(id), cancellationToken);

        return NoContent();
    }
    [HttpGet("member/{memberId:guid}")]
    [ProducesResponseType(typeof(List<MemberTicketDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MemberTicketDto>>> GetByMember(
        [FromRoute] Guid memberId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetMemberTicketsQuery(memberId),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SupportTicketDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SupportTicketDetailDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetTicketByIdQuery(id), cancellationToken);

        return Ok(result);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("open")]
    [ProducesResponseType(typeof(List<OpenTicketDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OpenTicketDto>>> GetOpen(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetOpenTicketsQuery(), cancellationToken);

        return Ok(result);
    }
}