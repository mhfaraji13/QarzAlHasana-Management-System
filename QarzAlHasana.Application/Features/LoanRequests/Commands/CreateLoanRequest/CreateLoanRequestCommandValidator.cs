using FluentValidation;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.CreateLoanRequest;

public class CreateLoanRequestCommandValidator : AbstractValidator<CreateLoanRequestCommand>
{
    public CreateLoanRequestCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Shenase-ye ozv elzami ast.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Mablagh bayad bozorgtar az sefr bashad.");

        RuleFor(x => x.InstallmentCount)
            .InclusiveBetween(1, 60)
            .WithMessage("Te'dad-e aghsat bayad beyn-e 1 ta 60 bashad.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Tozihat elzami ast.")
            .MaximumLength(500)
            .WithMessage("Tozihat nabayad bishtar az 500 karakter bashad.");
    }
}