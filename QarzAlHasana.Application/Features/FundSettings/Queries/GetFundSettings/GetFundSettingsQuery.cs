using MediatR;

namespace QarzAlHasana.Application.Features.FundSettings.Queries.GetFundSettings;

public record GetFundSettingsQuery : IRequest<FundSettingsDto>;