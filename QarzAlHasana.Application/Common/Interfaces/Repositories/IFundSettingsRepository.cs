using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Application.Common.Interfaces.Repositories;

public interface IFundSettingsRepository
{
    Task<FundSettings?> GetAsync(CancellationToken cancellationToken = default);

    Task<decimal> GetRegistrationFeeAsync(CancellationToken cancellationToken = default);

    Task<decimal> GetMonthlyMembershipFeeAsync(CancellationToken cancellationToken = default);

    Task AddAsync(FundSettings settings, CancellationToken cancellationToken = default);

    void Update(FundSettings settings);
}