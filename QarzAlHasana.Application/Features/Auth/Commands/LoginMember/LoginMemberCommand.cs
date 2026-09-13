using MediatR;
using QarzAlHasana.Application.Features.Auth.Common;

namespace QarzAlHasana.Application.Features.Auth.Commands.LoginMember;

public record LoginMemberCommand(
    string PhoneNumber,
    string Password) : IRequest<AuthResult>;