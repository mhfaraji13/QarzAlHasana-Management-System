using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Enums;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.SupportTickets.Commands.AddSupportMessage;

public class AddSupportMessageCommandHandler
    : IRequestHandler<AddSupportMessageCommand>
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public AddSupportMessageCommandHandler(
        ISupportTicketRepository supportTicketRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _supportTicketRepository = supportTicketRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        AddSupportMessageCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _supportTicketRepository
            .GetByIdWithMessagesAsync(request.TicketId, cancellationToken);

        if (ticket is null)
        {
            throw new NotFoundException(
                $"Ticket ba shenase {request.TicketId} peyda nashod.");
        }
        
        var senderType = _currentUserService.Role == Roles.Admin
            ? SenderType.Admin
            : SenderType.Member;

        ticket.AddMessage(request.Content, senderType);

        

        _supportTicketRepository.Update(ticket);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}