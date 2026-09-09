using FluentValidation;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.PayInstallment;

public sealed class PayInstallmentCommandValidator
    : AbstractValidator<PayInstallmentCommand>
{
    public PayInstallmentCommandValidator()
    {
        RuleFor(x => x.LoanRequestId)
            .NotEmpty()
            .WithMessage("Id-e- darkhast-e- vam elzami ast.");

        RuleFor(x => x.InstallmentId)
            .NotEmpty()
            .WithMessage("Id-e- ghest elzami ast.");

        RuleFor(x => x.AmountPaid)
            .GreaterThan(0)
            .WithMessage("Mablagh-e- pardakhti bayad bozorgtar az sefr bashad.");
    }
}