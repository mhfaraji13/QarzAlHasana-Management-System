using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetTicketById;

public class GetTicketByIdQueryHandler
    : IRequestHandler<GetTicketByIdQuery, SupportTicketDetailDto>
{
    private readonly ISupportTicketRepository _supportTicketRepository;

    public GetTicketByIdQueryHandler(ISupportTicketRepository supportTicketRepository)
    {
        _supportTicketRepository = supportTicketRepository;
    }

    public async Task<SupportTicketDetailDto> Handle(
        GetTicketByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _supportTicketRepository
            .GetTicketDetailAsync(request.TicketId, cancellationToken);

        if (result is null)
        {
            throw new NotFoundException(
                $"Ticket ba shenase {request.TicketId} peyda nashod.");
        }

        return result;
    }
}