using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Application.Common.Interfaces.Repositories;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Member?> GetByNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default);

    Task<Member?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

    Task<Member?> GetWithLoanRequestsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsByNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default);

    Task AddAsync(Member member, CancellationToken cancellationToken = default);

    void Update(Member member);

    void Remove(Member member);
}