using QarzAlHasana.Application.Features.Guarantors.Queries.GetPendingGuaranteeRequests;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Application.Common.Interfaces.Repositories;

public interface IGuarantorRepository
{
    Task<Guarantor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Guarantor?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Guarantor>> GetByLoanRequestIdAsync(Guid loanRequestId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Guarantor>> GetByGuarantorMemberIdAsync(Guid guarantorMemberId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Guarantor>> GetPendingConfirmationsAsync(Guid guarantorMemberId, CancellationToken cancellationToken = default);

    Task<decimal> GetTotalCommittedAmountAsync(Guid guarantorMemberId, CancellationToken cancellationToken = default);

    Task<bool> IsAlreadyGuarantorAsync(Guid loanRequestId, Guid guarantorMemberId, CancellationToken cancellationToken = default);
    Task<List<PendingGuaranteeRequestDto>> GetPendingForMemberAsync(Guid memberId, CancellationToken cancellationToken);

    Task AddAsync(Guarantor guarantor, CancellationToken cancellationToken = default);

    void Update(Guarantor guarantor);

    void Remove(Guarantor guarantor);
}