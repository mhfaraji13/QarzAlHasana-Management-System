using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetMemberTickets;

public class GetMemberTicketsQueryHandler
    : IRequestHandler<GetMemberTicketsQuery, List<MemberTicketDto>>
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMemberTicketsQueryHandler(ISupportTicketRepository supportTicketRepository, ICurrentUserService currentUserService)
    {
        _supportTicketRepository = supportTicketRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<MemberTicketDto>> Handle(
        GetMemberTicketsQuery request,
        CancellationToken cancellationToken)
    {
        var memberId = _currentUserService.UserId!.Value;
        return await _supportTicketRepository
            .GetTicketsByMemberAsync(memberId, cancellationToken);
    }
}