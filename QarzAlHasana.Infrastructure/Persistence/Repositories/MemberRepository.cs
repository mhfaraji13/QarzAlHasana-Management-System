using Microsoft.EntityFrameworkCore;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Application.Features.Members.Queries.GetInactiveMembers;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Infrastructure.Persistence.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext _context;

    public MemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Member?> GetByNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.NationalCode == nationalCode, cancellationToken);
    }

    public async Task<Member?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<Member?> GetWithLoanRequestsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .Include(m => m.LoanRequests)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .AsNoTracking()
            .OrderBy(m => m.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .AnyAsync(m => m.NationalCode == nationalCode, cancellationToken);
    }

    public async Task AddAsync(Member member, CancellationToken cancellationToken = default)
    {
        await _context.Members.AddAsync(member, cancellationToken);
    }
    public async Task<List<InactiveMemberDto>> GetInactiveMembersAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Members
            .AsNoTracking()
            .Where(m => !m.IsActive)
            .OrderBy(m => m.CreatedAt)
            .Select(m => new InactiveMemberDto
            {
                Id = m.Id,
                FullName = m.FirstName + " " + m.LastName,
                NationalCode = m.NationalCode,
                PhoneNumber = m.PhoneNumber,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }

    public void Update(Member member)
    {
        _context.Members.Update(member);
    }

    public void Remove(Member member)
    {
        member.IsDeleted = true;
        _context.Members.Update(member);
    }
}