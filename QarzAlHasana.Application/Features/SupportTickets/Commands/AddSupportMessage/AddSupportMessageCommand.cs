using MediatR;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.SupportTickets.Commands.AddSupportMessage;

public record AddSupportMessageCommand(
    Guid TicketId,
    string Content) : IRequest;