using MediatR;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetMemberLoanRequests;

public record GetMemberLoanRequestsQuery : IRequest<IReadOnlyList<LoanRequestListItemDto>>;