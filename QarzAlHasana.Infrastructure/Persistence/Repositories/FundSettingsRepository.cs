using Microsoft.EntityFrameworkCore;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Infrastructure.Persistence.Repositories;

public class FundSettingsRepository: IFundSettingsRepository
{
    private readonly ApplicationDbContext _context;

    public FundSettingsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FundSettings?> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _context.FundSettings
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<decimal> GetRegistrationFeeAsync(CancellationToken cancellationToken = default)
    {
        var settings = await _context.FundSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return settings?.RegistrationFee ?? 0m;
    }

    public async Task<decimal> GetMonthlyMembershipFeeAsync(CancellationToken cancellationToken = default)
    {
        var settings = await _context.FundSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return settings?.MonthlyMembershipFee ?? 0m;
    }

    public async Task AddAsync(FundSettings settings, CancellationToken cancellationToken = default)
    {
        await _context.FundSettings.AddAsync(settings, cancellationToken);
    }

    public void Update(FundSettings settings)
    {
        _context.FundSettings.Update(settings);
    }
}