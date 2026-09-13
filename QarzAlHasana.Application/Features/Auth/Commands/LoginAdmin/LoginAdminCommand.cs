using MediatR;
using QarzAlHasana.Application.Features.Auth.Common;

namespace QarzAlHasana.Application.Features.Auth.Commands.LoginAdmin;

public record LoginAdminCommand(
    string Email,
    string Password) : IRequest<AuthResult>;