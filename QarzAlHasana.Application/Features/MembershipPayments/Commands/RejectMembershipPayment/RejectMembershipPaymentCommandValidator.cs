using FluentValidation;

namespace QarzAlHasana.Application.Features.MembershipPayments.Commands.RejectMembershipPayment;

public class RejectMembershipPaymentCommandValidator
    : AbstractValidator<RejectMembershipPaymentCommand>
{
    public RejectMembershipPaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty();

        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .MaximumLength(500);
    }
}