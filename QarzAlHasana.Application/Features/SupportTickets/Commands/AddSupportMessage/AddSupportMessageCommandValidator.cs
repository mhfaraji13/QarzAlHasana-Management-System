using FluentValidation;

namespace QarzAlHasana.Application.Features.SupportTickets.Commands.AddSupportMessage;

public class AddSupportMessageCommandValidator
    : AbstractValidator<AddSupportMessageCommand>
{
    public AddSupportMessageCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(2000);

        
    }
}