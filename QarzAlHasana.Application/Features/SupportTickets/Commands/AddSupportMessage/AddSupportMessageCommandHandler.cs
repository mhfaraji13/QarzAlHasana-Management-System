using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.SupportTickets.Commands.AddSupportMessage;

public class AddSupportMessageCommandHandler
    : IRequestHandler<AddSupportMessageCommand>
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddSupportMessageCommandHandler(
        ISupportTicketRepository supportTicketRepository,
        IUnitOfWork unitOfWork)
    {
        _supportTicketRepository = supportTicketRepository;
        _unitOfWork = unitOfWork;
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

        ticket.AddMessage(request.Content, request.SenderType);

        _supportTicketRepository.Update(ticket);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}