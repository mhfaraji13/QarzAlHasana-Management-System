using MediatR;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasana.API.Contracts.Auth;
using QarzAlHasana.Application.Features.Auth.Commands.LoginAdmin;
using QarzAlHasana.Application.Features.Auth.Commands.LoginMember;
using QarzAlHasana.Application.Features.Auth.Commands.RegisterMember;
using QarzAlHasana.Application.Features.Auth.Common;

namespace QarzAlHasana.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterMemberRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterMemberCommand(
            request.FirstName,
            request.LastName,
            request.NationalCode,
            request.PhoneNumber,
            request.Password);

        var id = await _sender.Send(command, cancellationToken);

        return Ok(id);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResult>> Login(
        [FromBody] LoginMemberRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginMemberCommand(
            request.PhoneNumber,
            request.Password);

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost("admin/login")]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResult>> LoginAdmin(
        [FromBody] LoginAdminRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginAdminCommand(
            request.Email,
            request.Password);

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
}