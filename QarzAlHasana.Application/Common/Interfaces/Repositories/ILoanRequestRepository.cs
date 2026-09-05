using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Common.Interfaces.Repositories;

public interface ILoanRequestRepository
{
    Task<LoanRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<LoanRequest?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LoanRequest>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LoanRequest>> GetByStatusAsync(LoanStatus status, CancellationToken cancellationToken = default);

    Task<bool> HasActiveLoanAsync(Guid memberId, CancellationToken cancellationToken = default);

    Task AddAsync(LoanRequest loanRequest, CancellationToken cancellationToken = default);

    void Update(LoanRequest loanRequest);

    void Remove(LoanRequest loanRequest);
}