using MediatR;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanInstallments;

public sealed record GetLoanInstallmentsQuery(Guid LoanRequestId)
    : IRequest<LoanInstallmentsDto>;