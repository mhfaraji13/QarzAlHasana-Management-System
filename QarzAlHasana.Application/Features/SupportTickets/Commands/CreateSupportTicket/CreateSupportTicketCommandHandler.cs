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

    public CreateSupportTicketCommandHandler(
        ISupportTicketRepository supportTicketRepository,
        IUnitOfWork unitOfWork)
    {
        _supportTicketRepository = supportTicketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateSupportTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = SupportTicket.Create(
            request.MemberId,
            request.Subject,
            request.Content);

        await _supportTicketRepository.AddAsync(ticket, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ticket.Id;
    }
}