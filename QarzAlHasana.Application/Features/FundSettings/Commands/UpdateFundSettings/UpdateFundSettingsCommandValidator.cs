using FluentValidation;

namespace QarzAlHasana.Application.Features.FundSettings.Commands.UpdateFundSettings;

public class UpdateFundSettingsCommandValidator
    : AbstractValidator<UpdateFundSettingsCommand>
{
    public UpdateFundSettingsCommandValidator()
    {
        RuleFor(x => x.RegistrationFee)
            .GreaterThan(0)
            .WithMessage("Haghe-e sabt-e nam bayad bozorgtar az sefr bashad.");

        RuleFor(x => x.MonthlyMembershipFee)
            .GreaterThan(0)
            .WithMessage("Haghe-e ozviyat-e mahane bayad bozorgtar az sefr bashad.");
    }
}