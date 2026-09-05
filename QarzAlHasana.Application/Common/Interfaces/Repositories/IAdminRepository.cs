using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Application.Common.Interfaces.Repositories;

public interface IAdminRepository
{
    Task<Admin?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Admin?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    Task<Admin?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Admin>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Admin admin, CancellationToken cancellationToken = default);

    void Update(Admin admin);

    void Remove(Admin admin);
}