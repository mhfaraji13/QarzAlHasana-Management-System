using MediatR;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetTicketById;

public record GetTicketByIdQuery(Guid TicketId) : IRequest<SupportTicketDetailDto>;