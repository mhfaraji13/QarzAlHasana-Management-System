using MediatR;

namespace QarzAlHasana.Application.Features.Members.Queries.GetInactiveMembers;

public record GetInactiveMembersQuery : IRequest<List<InactiveMemberDto>>;