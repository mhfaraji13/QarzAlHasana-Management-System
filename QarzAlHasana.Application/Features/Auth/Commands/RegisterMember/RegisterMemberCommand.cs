using MediatR;

namespace QarzAlHasana.Application.Features.Auth.Commands.RegisterMember;

public record RegisterMemberCommand(
    string FirstName,
    string LastName,
    string NationalCode,
    string PhoneNumber,
    string Password) : IRequest<Guid>;