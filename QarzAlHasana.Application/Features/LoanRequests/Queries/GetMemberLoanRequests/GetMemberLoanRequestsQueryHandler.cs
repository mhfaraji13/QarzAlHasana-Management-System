using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetMemberLoanRequests;

public class GetMemberLoanRequestsQueryHandler
    : IRequestHandler<GetMemberLoanRequestsQuery, IReadOnlyList<LoanRequestListItemDto>>
{
    private readonly ILoanRequestRepository _loanRequestRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMemberLoanRequestsQueryHandler(
        ILoanRequestRepository loanRequestRepository,
        ICurrentUserService currentUserService)
    {
        _loanRequestRepository = loanRequestRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<LoanRequestListItemDto>> Handle(
        GetMemberLoanRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var memberId = _currentUserService.UserId!.Value;

        var loanRequests = await _loanRequestRepository
            .GetByMemberIdAsync(memberId, cancellationToken);

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