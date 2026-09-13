using MediatR;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetOpenTickets;

public record GetOpenTicketsQuery : IRequest<List<OpenTicketDto>>;