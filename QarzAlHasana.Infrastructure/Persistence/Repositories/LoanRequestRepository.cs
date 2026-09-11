using Microsoft.EntityFrameworkCore;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanRequestById;
using QarzAlHasana.Application.Features.LoanRequests.Queries.GetPendingLoanRequests;
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

    public async Task<List<PendingLoanRequestDto>> GetPendingAsync(CancellationToken cancellationToken)
    {
        return await _context.LoanRequests
            .AsNoTracking()
            .Where(lr => lr.Status == LoanStatus.Pending)
            .OrderBy(lr => lr.RequestDate)
            .Select(lr => new PendingLoanRequestDto
            {
                Id = lr.Id,
                MemberId = lr.MemberId,
                MemberFullName = lr.Member.FirstName + " " + lr.Member.LastName,
                Amount = lr.Amount,
                InstallmentCount = lr.InstallmentCount,
                Description = lr.Description,
                RequestDate = lr.RequestDate
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<LoanRequestDetailDto?> GetDetailByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.LoanRequests
            .AsNoTracking()
            .Where(lr => lr.Id == id)
            .Select(lr => new LoanRequestDetailDto
            {
                Id = lr.Id,
                Amount = lr.Amount,
                InstallmentCount = lr.InstallmentCount,
                Description = lr.Description,
                Status = lr.Status,
                RequestDate = lr.RequestDate,
                ReviewedDate = lr.ReviewedDate,
                RejectionReason = lr.RejectionReason,

                MemberId = lr.MemberId,
                MemberFullName = lr.Member.FirstName + " " + lr.Member.LastName,
                MemberNationalCode = lr.Member.NationalCode,
                MemberPhoneNumber = lr.Member.PhoneNumber,

                Guarantors = lr.Guarantors
                    .Select(g => new LoanGuarantorDto
                    {
                        Id = g.Id,
                        GuarantorMemberId = g.GuarantorMemberId,
                        GuarantorFullName = g.GuarantorMember.FirstName + " " + g.GuarantorMember.LastName,
                        GuarantorPhoneNumber = g.GuarantorMember.PhoneNumber,
                        CommittedAmount = g.CommittedAmount,
                        IsConfirmed = g.IsConfirmed,
                        ConfirmedDate = g.ConfirmedDate
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
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