using Microsoft.EntityFrameworkCore;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Infrastructure.Persistence.Repositories;

public class GuarantorRepository : IGuarantorRepository
{
    private readonly ApplicationDbContext _context;

    public GuarantorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

   public async Task<Guarantor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Guarantors
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<Guarantor?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Guarantors
            .Include(g => g.GuarantorMember)
            .Include(g => g.LoanRequest)
                .ThenInclude(lr => lr.Member)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Guarantor>> GetByLoanRequestIdAsync(Guid loanRequestId, CancellationToken cancellationToken = default)
    {
        return await _context.Guarantors
            .AsNoTracking()
            .Include(g => g.GuarantorMember)
            .Where(g => g.LoanRequestId == loanRequestId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Guarantor>> GetByGuarantorMemberIdAsync(Guid guarantorMemberId, CancellationToken cancellationToken = default)
    {
        return await _context.Guarantors
            .AsNoTracking()
            .Include(g => g.LoanRequest)
                .ThenInclude(lr => lr.Member)
            .Where(g => g.GuarantorMemberId == guarantorMemberId)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Guarantor>> GetPendingConfirmationsAsync(Guid guarantorMemberId, CancellationToken cancellationToken = default)
    {
        return await _context.Guarantors
            .AsNoTracking()
            .Include(g => g.LoanRequest)
                .ThenInclude(lr => lr.Member)
            .Where(g => g.GuarantorMemberId == guarantorMemberId
                        && !g.IsConfirmed
                        && g.LoanRequest.Status == LoanStatus.Pending)
            .OrderBy(g => g.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalCommittedAmountAsync(Guid guarantorMemberId, CancellationToken cancellationToken = default)
    {
        return await _context.Guarantors
            .Where(g => g.GuarantorMemberId == guarantorMemberId
                        && g.IsConfirmed
                        && (g.LoanRequest.Status == LoanStatus.Pending
                            || g.LoanRequest.Status == LoanStatus.Approved))
            .SumAsync(g => g.CommittedAmount, cancellationToken);
    }

    public async Task<bool> IsAlreadyGuarantorAsync(Guid loanRequestId, Guid guarantorMemberId, CancellationToken cancellationToken = default)
    {
        return await _context.Guarantors
            .AnyAsync(g => g.LoanRequestId == loanRequestId
                           && g.GuarantorMemberId == guarantorMemberId,
                      cancellationToken);
    }

    public async Task AddAsync(Guarantor guarantor, CancellationToken cancellationToken = default)
    {
        await _context.Guarantors.AddAsync(guarantor, cancellationToken);
    }

    public void Update(Guarantor guarantor)
    {
        _context.Guarantors.Update(guarantor);
    }

    public void Remove(Guarantor guarantor)
    {
        guarantor.IsDeleted = true;
        _context.Guarantors.Update(guarantor);
    }
}