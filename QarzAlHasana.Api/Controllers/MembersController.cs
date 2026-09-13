using MediatR;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasana.Application.Features.Members.Commands.ActivateMember;
using QarzAlHasana.Application.Features.Members.Queries.GetInactiveMembers;

namespace QarzAlHasana.Api.Controllers;

[ApiController]
[Route("api/members")]
public sealed class MembersController : ControllerBase
{
    private readonly ISender _sender;

    public MembersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("inactive")]
    [ProducesResponseType(typeof(List<InactiveMemberDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<InactiveMemberDto>>> GetInactive(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetInactiveMembersQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Activate(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new ActivateMemberCommand(id), cancellationToken);

        return NoContent();
    }
}