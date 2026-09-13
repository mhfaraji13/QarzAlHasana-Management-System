using MediatR;

namespace QarzAlHasana.Application.Features.SupportTickets.Commands.CreateSupportTicket;

public record CreateSupportTicketCommand(
    string Subject,
    string Content) : IRequest<Guid>;