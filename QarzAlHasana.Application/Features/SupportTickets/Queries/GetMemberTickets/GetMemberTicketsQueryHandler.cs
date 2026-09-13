using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetMemberTickets;

public class GetMemberTicketsQueryHandler
    : IRequestHandler<GetMemberTicketsQuery, List<MemberTicketDto>>
{
    private readonly ISupportTicketRepository _supportTicketRepository;

    public GetMemberTicketsQueryHandler(ISupportTicketRepository supportTicketRepository)
    {
        _supportTicketRepository = supportTicketRepository;
    }

    public async Task<List<MemberTicketDto>> Handle(
        GetMemberTicketsQuery request,
        CancellationToken cancellationToken)
    {
        return await _supportTicketRepository
            .GetTicketsByMemberAsync(request.MemberId, cancellationToken);
    }
}