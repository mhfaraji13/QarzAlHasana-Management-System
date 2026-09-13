using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Common;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetTicketById;

public class GetTicketByIdQueryHandler
    : IRequestHandler<GetTicketByIdQuery, SupportTicketDetailDto>
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTicketByIdQueryHandler(
        ISupportTicketRepository supportTicketRepository,
        ICurrentUserService currentUserService)
    {
        _supportTicketRepository = supportTicketRepository;
        _currentUserService = currentUserService;
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

        if (_currentUserService.Role != Roles.Admin &&
            result.MemberId != _currentUserService.UserId)
        {
            throw new ForbiddenException("Shoma be in ticket dastresi nadarid.");
        }

        return result;
    }
}