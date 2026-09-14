using FluentValidation;

namespace QarzAlHasana.Application.Features.Guarantors.Commands.ConfirmGuarantor;

public class ConfirmGuarantorCommandValidator : AbstractValidator<ConfirmGuarantorCommand>
{
    public ConfirmGuarantorCommandValidator()
    {
        RuleFor(x => x.LoanRequestId).NotEmpty();

        RuleFor(x => x.GuarantorId).NotEmpty();
    }
}