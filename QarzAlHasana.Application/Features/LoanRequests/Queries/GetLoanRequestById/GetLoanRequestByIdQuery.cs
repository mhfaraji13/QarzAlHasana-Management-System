using MediatR;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanRequestById;

public record GetLoanRequestByIdQuery(Guid Id) : IRequest<LoanRequestDetailDto>;