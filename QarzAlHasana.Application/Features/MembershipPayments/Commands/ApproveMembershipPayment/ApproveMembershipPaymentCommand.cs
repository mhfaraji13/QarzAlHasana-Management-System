using MediatR;

namespace QarzAlHasana.Application.Features.MembershipPayments.Commands.ApproveMembershipPayment;

public record ApproveMembershipPaymentCommand(Guid PaymentId) : IRequest;