using MediatR;

namespace QarzAlHasana.Application.Features.MembershipPayments.Commands.RejectMembershipPayment;

public record RejectMembershipPaymentCommand(
    Guid PaymentId,
    string RejectionReason) : IRequest;