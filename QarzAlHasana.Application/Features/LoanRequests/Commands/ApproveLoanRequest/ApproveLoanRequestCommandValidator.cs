using FluentValidation;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.ApproveLoanRequest;

public sealed class ApproveLoanRequestCommandValidator
    : AbstractValidator<ApproveLoanRequestCommand>
{
    public ApproveLoanRequestCommandValidator()
    {
        RuleFor(x => x.LoanRequestId)
            .NotEmpty()
            .WithMessage("Id-e- darkhast-e- vam elzami ast.");
    }
}