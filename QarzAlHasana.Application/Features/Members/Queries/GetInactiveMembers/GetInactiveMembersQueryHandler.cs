using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;

namespace QarzAlHasana.Application.Features.Members.Queries.GetInactiveMembers;

public class GetInactiveMembersQueryHandler
    : IRequestHandler<GetInactiveMembersQuery, List<InactiveMemberDto>>
{
    private readonly IMemberRepository _memberRepository;

    public GetInactiveMembersQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<List<InactiveMemberDto>> Handle(
        GetInactiveMembersQuery request,
        CancellationToken cancellationToken)
    {
        return await _memberRepository.GetInactiveMembersAsync(cancellationToken);
    }
}