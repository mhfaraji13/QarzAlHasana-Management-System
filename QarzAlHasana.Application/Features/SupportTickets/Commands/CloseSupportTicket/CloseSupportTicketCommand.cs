using MediatR;

namespace QarzAlHasana.Application.Features.SupportTickets.Commands.CloseSupportTicket;

public record CloseSupportTicketCommand(Guid TicketId) : IRequest;