using FluentValidation;

namespace QarzAlHasana.Application.Features.Guarantors.Commands.AddGuarantor;

public class AddGuarantorCommandValidator : AbstractValidator<AddGuarantorCommand>
{
    public AddGuarantorCommandValidator()
    {
        RuleFor(x => x.LoanRequestId).NotEmpty();

        RuleFor(x => x.GuarantorMemberId).NotEmpty();

        RuleFor(x => x.CommittedAmount)
            .GreaterThan(0)
            .WithMessage("Mablagh-e zemanat bayad bozorgtar az sefr bashad.");
    }
}