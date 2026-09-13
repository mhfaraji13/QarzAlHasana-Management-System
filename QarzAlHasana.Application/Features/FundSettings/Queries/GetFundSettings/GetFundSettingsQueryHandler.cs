using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.FundSettings.Queries.GetFundSettings;

public class GetFundSettingsQueryHandler
    : IRequestHandler<GetFundSettingsQuery, FundSettingsDto>
{
    private readonly IFundSettingsRepository _fundSettingsRepository;

    public GetFundSettingsQueryHandler(IFundSettingsRepository fundSettingsRepository)
    {
        _fundSettingsRepository = fundSettingsRepository;
    }

    public async Task<FundSettingsDto> Handle(
        GetFundSettingsQuery request,
        CancellationToken cancellationToken)
    {
        var settings = await _fundSettingsRepository.GetAsync(cancellationToken);

        if (settings is null)
        {
            throw new NotFoundException(
                "Tanzimat-e sandogh hanooz sabt nashode ast.");
        }

        return new FundSettingsDto
        {
            Id = settings.Id,
            RegistrationFee = settings.RegistrationFee,
            MonthlyMembershipFee = settings.MonthlyMembershipFee,
            UpdatedAt = settings.UpdatedAt
        };
    }
}