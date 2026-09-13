using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetOpenTickets;

public class GetOpenTicketsQueryHandler
    : IRequestHandler<GetOpenTicketsQuery, List<OpenTicketDto>>
{
    private readonly ISupportTicketRepository _supportTicketRepository;

    public GetOpenTicketsQueryHandler(ISupportTicketRepository supportTicketRepository)
    {
        _supportTicketRepository = supportTicketRepository;
    }

    public async Task<List<OpenTicketDto>> Handle(
        GetOpenTicketsQuery request,
        CancellationToken cancellationToken)
    {
        return await _supportTicketRepository.GetOpenTicketsAsync(cancellationToken);
    }
}