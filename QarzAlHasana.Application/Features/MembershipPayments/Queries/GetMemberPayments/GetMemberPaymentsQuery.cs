using MediatR;

namespace QarzAlHasana.Application.Features.MembershipPayments.Queries.GetMemberPayments;

public record GetMemberPaymentsQuery(Guid MemberId) : IRequest<List<MemberPaymentDto>>;