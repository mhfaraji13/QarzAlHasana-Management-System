using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;

namespace QarzAlHasana.Application.Features.MembershipPayments.Queries.GetMemberPayments;

public class GetMemberPaymentsQueryHandler
    : IRequestHandler<GetMemberPaymentsQuery, List<MemberPaymentDto>>
{
    private readonly IMembershipPaymentRepository _membershipPaymentRepository;

    public GetMemberPaymentsQueryHandler(
        IMembershipPaymentRepository membershipPaymentRepository)
    {
        _membershipPaymentRepository = membershipPaymentRepository;
    }

    public async Task<List<MemberPaymentDto>> Handle(
        GetMemberPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _membershipPaymentRepository
            .GetPaymentsByMemberAsync(request.MemberId, cancellationToken);
    }
}