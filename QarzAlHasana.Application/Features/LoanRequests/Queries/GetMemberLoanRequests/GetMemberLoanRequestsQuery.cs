using MediatR;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetMemberLoanRequests;

public record GetMemberLoanRequestsQuery(Guid MemberId): IRequest<IReadOnlyList<LoanRequestListItemDto>>;
