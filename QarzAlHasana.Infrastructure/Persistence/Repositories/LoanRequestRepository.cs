using Microsoft.EntityFrameworkCore;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Infrastructure.Persistence.Repositories;

public class LoanRequestRepository : ILoanRequestRepository
{
    private readonly ApplicationDbContext _context;

    public LoanRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LoanRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.LoanRequests
            .FirstOrDefaultAsync(lr => lr.Id == id, cancellationToken);
    }

    public async Task<LoanRequest?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.LoanRequests
            .Include(lr => lr.Member)
            .Include(lr => lr.Installments)
            .Include(lr => lr.Guarantors)
            .ThenInclude(g => g.GuarantorMember)
            .FirstOrDefaultAsync(lr => lr.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<LoanRequest>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.LoanRequests
            .AsNoTracking()
            .Where(lr => lr.MemberId == memberId)
            .OrderByDescending(lr => lr.RequestDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<LoanRequest>> GetByStatusAsync(LoanStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.LoanRequests
            .AsNoTracking()
            .Include(lr => lr.Member)
            .Where(lr => lr.Status == status)
            .OrderBy(lr => lr.RequestDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasActiveLoanAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.LoanRequests
            .AnyAsync(lr => lr.MemberId == memberId
                            && (lr.Status == LoanStatus.Pending || lr.Status == LoanStatus.Approved),
                cancellationToken);
    }

    public async Task<LoanRequest?> GetByIdWithInstallmentsAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.LoanRequests
            .Include(l => l.Installments)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task AddAsync(LoanRequest loanRequest, CancellationToken cancellationToken = default)
    {
        await _context.LoanRequests.AddAsync(loanRequest, cancellationToken);
    }

    public void Update(LoanRequest loanRequest)
    {
        _context.LoanRequests.Update(loanRequest);
    }

    public void Remove(LoanRequest loanRequest)
    {
        loanRequest.IsDeleted = true;
        _context.LoanRequests.Update(loanRequest);
    }
}