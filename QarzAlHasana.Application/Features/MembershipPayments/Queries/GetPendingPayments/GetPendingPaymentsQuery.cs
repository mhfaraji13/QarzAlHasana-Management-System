using MediatR;

namespace QarzAlHasana.Application.Features.MembershipPayments.Queries.GetPendingPayments;

public record GetPendingPaymentsQuery : IRequest<List<PendingPaymentDto>>;