using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;

namespace QarzAlHasana.Application.Features.MembershipPayments.Queries.GetMemberPayments;

public class GetMemberPaymentsQueryHandler
    : IRequestHandler<GetMemberPaymentsQuery, List<MemberPaymentDto>>
{
    private readonly IMembershipPaymentRepository _membershipPaymentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMemberPaymentsQueryHandler(
        IMembershipPaymentRepository membershipPaymentRepository,
    ICurrentUserService currentUserService)
    {
        _membershipPaymentRepository = membershipPaymentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<MemberPaymentDto>> Handle(
        GetMemberPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var memberId = _currentUserService.UserId!.Value;

        return await _membershipPaymentRepository
            .GetPaymentsByMemberAsync(memberId, cancellationToken);
    }
}