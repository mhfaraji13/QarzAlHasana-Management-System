using FluentValidation;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.RejectLoanRequest;

public class RejectLoanRequestCommandValidator : AbstractValidator<RejectLoanRequestCommand>
{
    public RejectLoanRequestCommandValidator()
    {
        RuleFor(x => x.LoanRequestId)
            .NotEmpty();

        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .MaximumLength(1000);
    }
}