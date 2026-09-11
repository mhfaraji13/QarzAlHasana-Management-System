using MediatR;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetPendingLoanRequests;

public record GetPendingLoanRequestsQuery : IRequest<List<PendingLoanRequestDto>>;