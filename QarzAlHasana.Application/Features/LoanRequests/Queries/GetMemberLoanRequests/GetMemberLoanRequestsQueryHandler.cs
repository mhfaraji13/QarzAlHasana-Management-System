using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetMemberLoanRequests;

public class GetMemberLoanRequestsQueryHandler : IRequestHandler<GetMemberLoanRequestsQuery, IReadOnlyList<LoanRequestListItemDto>>
{
    private readonly ILoanRequestRepository _loanRequestRepository;

    public GetMemberLoanRequestsQueryHandler(ILoanRequestRepository  loanRequestRepository)
    {
        _loanRequestRepository = loanRequestRepository;
    }

    public async Task<IReadOnlyList<LoanRequestListItemDto>> Handle(GetMemberLoanRequestsQuery request, CancellationToken cancellationToken)
    {
        var loanRequests = await _loanRequestRepository
            .GetByMemberIdAsync(request.MemberId, cancellationToken);

        return loanRequests
            .Select(lr => new LoanRequestListItemDto(
                lr.Id,
                lr.Amount,
                lr.InstallmentCount,
                lr.Description,
                lr.Status.ToString(),
                lr.RequestDate,
                lr.ReviewedDate,
                lr.RejectionReason))
            .ToList();
    }
}