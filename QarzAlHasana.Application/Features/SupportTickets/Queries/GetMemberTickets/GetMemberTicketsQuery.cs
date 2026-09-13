using MediatR;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetMemberTickets;

public record GetMemberTicketsQuery(Guid MemberId) : IRequest<List<MemberTicketDto>>;