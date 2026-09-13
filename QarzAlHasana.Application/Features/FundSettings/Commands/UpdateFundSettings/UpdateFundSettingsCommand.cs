using MediatR;

namespace QarzAlHasana.Application.Features.FundSettings.Commands.UpdateFundSettings;

public record UpdateFundSettingsCommand(
    decimal RegistrationFee,
    decimal MonthlyMembershipFee) : IRequest;