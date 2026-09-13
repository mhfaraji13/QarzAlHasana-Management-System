using FluentValidation;

namespace QarzAlHasana.Application.Features.Auth.Commands.RegisterMember;

public class RegisterMemberCommandValidator
    : AbstractValidator<RegisterMemberCommand>
{
    public RegisterMemberCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.NationalCode)
            .NotEmpty()
            .Length(10)
            .Matches("^[0-9]{10}$");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches("^09[0-9]{9}$");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(100);
    }
}