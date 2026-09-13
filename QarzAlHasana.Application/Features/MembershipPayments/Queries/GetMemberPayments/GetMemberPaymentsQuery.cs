using MediatR;

namespace QarzAlHasana.Application.Features.MembershipPayments.Queries.GetMemberPayments;

public record GetMemberPaymentsQuery : IRequest<List<MemberPaymentDto>>;