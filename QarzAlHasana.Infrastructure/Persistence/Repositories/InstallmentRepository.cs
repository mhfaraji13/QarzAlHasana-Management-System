using Microsoft.EntityFrameworkCore;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Infrastructure.Persistence.Repositories;

public class InstallmentRepository : IInstallmentRepository
{
    private readonly ApplicationDbContext _context;

    public InstallmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Installment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Installments
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Installment>> GetByLoanRequestIdAsync(Guid loanRequestId, CancellationToken cancellationToken = default)
    {
        return await _context.Installments
            .AsNoTracking()
            .Where(i => i.LoanRequestId == loanRequestId)
            .OrderBy(i => i.InstallmentNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Installment>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.Installments
            .AsNoTracking()
            .Include(i => i.LoanRequest)
            .Where(i => i.LoanRequest.MemberId == memberId)
            .OrderBy(i => i.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Installment>> GetOverdueInstallmentsAsync(DateTime asOfDate, CancellationToken cancellationToken = default)
    {
        return await _context.Installments
            .AsNoTracking()
            .Include(i => i.LoanRequest)
                .ThenInclude(lr => lr.Member)
            .Where(i => i.Status != InstallmentStatus.Paid && i.DueDate < asOfDate)
            .OrderBy(i => i.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Installment>> GetUpcomingInstallmentsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        return await _context.Installments
            .AsNoTracking()
            .Include(i => i.LoanRequest)
                .ThenInclude(lr => lr.Member)
            .Where(i => i.Status == InstallmentStatus.Unpaid
                        && i.DueDate >= fromDate
                        && i.DueDate <= toDate)
            .OrderBy(i => i.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Installment?> GetNextUnpaidInstallmentAsync(Guid loanRequestId, CancellationToken cancellationToken = default)
    {
        return await _context.Installments
            .Where(i => i.LoanRequestId == loanRequestId
                        && i.Status != InstallmentStatus.Paid)
            .OrderBy(i => i.InstallmentNumber)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalPaidAmountAsync(Guid loanRequestId, CancellationToken cancellationToken = default)
    {
        return await _context.Installments
            .Where(i => i.LoanRequestId == loanRequestId
                        && i.Status == InstallmentStatus.Paid)
            .SumAsync(i => i.Amount, cancellationToken);
    }

    public async Task<decimal> GetRemainingAmountAsync(Guid loanRequestId, CancellationToken cancellationToken = default)
    {
        return await _context.Installments
            .Where(i => i.LoanRequestId == loanRequestId
                        && i.Status != InstallmentStatus.Paid)
            .SumAsync(i => i.Amount, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Installment> installments, CancellationToken cancellationToken = default)
    {
        await _context.Installments.AddRangeAsync(installments, cancellationToken);
    }

    public void Update(Installment installment)
    {
        _context.Installments.Update(installment);
    }

    public void UpdateRange(IEnumerable<Installment> installments)
    {
        _context.Installments.UpdateRange(installments);
    }
}