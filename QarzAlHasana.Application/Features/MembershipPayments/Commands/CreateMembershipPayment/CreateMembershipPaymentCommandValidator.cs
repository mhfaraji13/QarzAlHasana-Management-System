using FluentValidation;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.MembershipPayments.Commands.CreateMembershipPayment;

public class CreateMembershipPaymentCommandValidator
    : AbstractValidator<CreateMembershipPaymentCommand>
{
    public CreateMembershipPaymentCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty();

        RuleFor(x => x.Type)
            .IsInEnum();

        When(x => x.Type == MembershipPaymentType.Monthly, () =>
        {
            RuleFor(x => x.ForMonth)
                .NotNull()
                .InclusiveBetween(1, 12);

            RuleFor(x => x.ForYear)
                .NotNull()
                .InclusiveBetween(2000, 2100);
        });

        RuleFor(x => x.ReceiptImageUrl)
            .MaximumLength(500);
    }
}