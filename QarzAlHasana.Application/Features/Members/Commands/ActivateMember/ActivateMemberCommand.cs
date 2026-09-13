using MediatR;

namespace QarzAlHasana.Application.Features.Members.Commands.ActivateMember;

public record ActivateMemberCommand(Guid MemberId) : IRequest;