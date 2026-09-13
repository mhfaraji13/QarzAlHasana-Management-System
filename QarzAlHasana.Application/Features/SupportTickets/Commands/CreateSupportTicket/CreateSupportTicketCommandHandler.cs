using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Application.Features.SupportTickets.Commands.CreateSupportTicket;

public class CreateSupportTicketCommandHandler
    : IRequestHandler<CreateSupportTicketCommand, Guid>
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateSupportTicketCommandHandler(
        ISupportTicketRepository supportTicketRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService  currentUserService)
    {
        _supportTicketRepository = supportTicketRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(
        CreateSupportTicketCommand request,
        CancellationToken cancellationToken)
    {
        var memberId = _currentUserService.UserId!.Value;
        var ticket = SupportTicket.Create(
            memberId,
            request.Subject,
            request.Content);

        await _supportTicketRepository.AddAsync(ticket, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ticket.Id;
    }
}