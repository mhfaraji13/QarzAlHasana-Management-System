using Microsoft.EntityFrameworkCore;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Infrastructure.Persistence.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _context;

    public AdminRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Admin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Admins
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Admin?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Admins
            .FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
    }

    public async Task<Admin?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Admins
            .FirstOrDefaultAsync(a => a.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Admins
            .AnyAsync(a => a.Email == email, cancellationToken);
    }

    public async Task<IReadOnlyList<Admin>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Admins
            .AsNoTracking()
            .OrderBy(a => a.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Admin admin, CancellationToken cancellationToken = default)
    {
        await _context.Admins.AddAsync(admin, cancellationToken);
    }

    public void Update(Admin admin) => _context.Admins.Update(admin);
    

    public void Remove(Admin admin)
    {
        admin.IsDeleted = true;
        _context.Admins.Update(admin);
    }
}