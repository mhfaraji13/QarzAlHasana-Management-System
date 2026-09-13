using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasana.API.Contracts.FundSettings;
using QarzAlHasana.Application.Features.FundSettings.Commands.UpdateFundSettings;
using QarzAlHasana.Application.Features.FundSettings.Queries.GetFundSettings;

namespace QarzAlHasana.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/fund-settings")]
public sealed class FundSettingsController : ControllerBase
{
    private readonly ISender _sender;

    public FundSettingsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(FundSettingsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FundSettingsDto>> Get(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetFundSettingsQuery(), cancellationToken);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateFundSettingsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateFundSettingsCommand(
            request.RegistrationFee,
            request.MonthlyMembershipFee);

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}