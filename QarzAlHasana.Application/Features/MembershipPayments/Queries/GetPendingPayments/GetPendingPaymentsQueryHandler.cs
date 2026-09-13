using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;

namespace QarzAlHasana.Application.Features.MembershipPayments.Queries.GetPendingPayments;

public class GetPendingPaymentsQueryHandler
    : IRequestHandler<GetPendingPaymentsQuery, List<PendingPaymentDto>>
{
    private readonly IMembershipPaymentRepository _membershipPaymentRepository;

    public GetPendingPaymentsQueryHandler(
        IMembershipPaymentRepository membershipPaymentRepository)
    {
        _membershipPaymentRepository = membershipPaymentRepository;
    }

    public async Task<List<PendingPaymentDto>> Handle(
        GetPendingPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _membershipPaymentRepository
            .GetPendingPaymentsAsync(cancellationToken);
    }
}