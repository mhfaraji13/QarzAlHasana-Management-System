using MediatR;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetMemberTickets;

public record GetMemberTicketsQuery : IRequest<List<MemberTicketDto>>;