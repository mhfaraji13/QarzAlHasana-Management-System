using FluentValidation;

namespace QarzAlHasana.Application.Features.SupportTickets.Commands.CreateSupportTicket;

public class CreateSupportTicketCommandValidator
    : AbstractValidator<CreateSupportTicketCommand>
{
    public CreateSupportTicketCommandValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty();

        RuleFor(x => x.Subject)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(2000);
    }
}