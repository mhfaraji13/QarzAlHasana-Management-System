using MediatR;

namespace QarzAlHasana.Application.Features.SupportTickets.Commands.CreateSupportTicket;

public record CreateSupportTicketCommand(
    Guid MemberId,
    string Subject,
    string Content) : IRequest<Guid>;