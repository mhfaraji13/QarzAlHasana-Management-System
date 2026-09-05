using Microsoft.EntityFrameworkCore;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Infrastructure.Persistence.Repositories;

public class MembershipPaymentRepository : IMembershipPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public MembershipPaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MembershipPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPayments
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<MembershipPayment?> GetByIdWithMemberAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPayments
            .Include(p => p.Member)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<MembershipPayment>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPayments
            .AsNoTracking()
            .Where(p => p.MemberId == memberId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MembershipPayment>> GetByStatusAsync(DepositStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPayments
            .AsNoTracking()
            .Include(p => p.Member)
            .Where(p => p.Status == status)
            .OrderBy(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<MembershipPayment?> GetRegistrationPaymentAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPayments
            .FirstOrDefaultAsync(p => p.MemberId == memberId
                                      && p.Type == MembershipPaymentType.Registration,
                                 cancellationToken);
    }

    public async Task<bool> HasPaidForMonthAsync(Guid memberId, int year, int month, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPayments
            .AnyAsync(p => p.MemberId == memberId
                           && p.Type == MembershipPaymentType.Monthly
                           && p.Status == DepositStatus.Confirmed
                           && p.ForYear == year
                           && p.ForMonth == month,
                      cancellationToken);
    }

    public async Task<decimal> GetTotalConfirmedAmountAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPayments
            .Where(p => p.MemberId == memberId
                        && p.Status == DepositStatus.Confirmed)
            .SumAsync(p => p.Amount, cancellationToken);
    }

    public async Task<decimal> GetFundTotalBalanceAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPayments
            .Where(p => p.Status == DepositStatus.Confirmed)
            .SumAsync(p => p.Amount, cancellationToken);
    }

    public async Task<int> GetPendingCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPayments
            .CountAsync(p => p.Status == DepositStatus.Pending, cancellationToken);
    }

    public async Task AddAsync(MembershipPayment payment, CancellationToken cancellationToken = default)
    {
        await _context.MembershipPayments.AddAsync(payment, cancellationToken);
    }

    public void Update(MembershipPayment payment)
    {
        _context.MembershipPayments.Update(payment);
    }

    public void Remove(MembershipPayment payment)
    {
        payment.IsDeleted = true;
        _context.MembershipPayments.Update(payment);
    }
}