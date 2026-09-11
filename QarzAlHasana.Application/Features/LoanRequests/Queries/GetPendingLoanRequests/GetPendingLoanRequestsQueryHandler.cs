using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Application.Common.Interfaces;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetPendingLoanRequests;

public class GetPendingLoanRequestsQueryHandler
    : IRequestHandler<GetPendingLoanRequestsQuery, List<PendingLoanRequestDto>>
{
    private readonly ILoanRequestRepository _loanRequestRepository;

    public GetPendingLoanRequestsQueryHandler(ILoanRequestRepository loanRequestRepository)
    {
        _loanRequestRepository = loanRequestRepository;
    }

    public async Task<List<PendingLoanRequestDto>> Handle(
        GetPendingLoanRequestsQuery request,
        CancellationToken cancellationToken)
    {
        return await _loanRequestRepository.GetPendingAsync(cancellationToken);
    }
}