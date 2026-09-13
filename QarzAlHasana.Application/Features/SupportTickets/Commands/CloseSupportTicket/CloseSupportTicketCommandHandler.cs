using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.SupportTickets.Commands.CloseSupportTicket;

public class CloseSupportTicketCommandHandler
    : IRequestHandler<CloseSupportTicketCommand>
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseSupportTicketCommandHandler(
        ISupportTicketRepository supportTicketRepository,
        IUnitOfWork unitOfWork)
    {
        _supportTicketRepository = supportTicketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        CloseSupportTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _supportTicketRepository
            .GetByIdAsync(request.TicketId, cancellationToken);

        if (ticket is null)
        {
            throw new NotFoundException(
                $"Ticket ba shenase {request.TicketId} peyda nashod.");
        }

        ticket.Close();

        _supportTicketRepository.Update(ticket);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}