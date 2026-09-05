using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Application.Common.Interfaces.Repositories;

public interface IInstallmentRepository
{
    Task<Installment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Installment>> GetByLoanRequestIdAsync(Guid loanRequestId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Installment>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Installment>> GetOverdueInstallmentsAsync(DateTime asOfDate, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Installment>> GetUpcomingInstallmentsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);

    Task<Installment?> GetNextUnpaidInstallmentAsync(Guid loanRequestId, CancellationToken cancellationToken = default);

    Task<decimal> GetTotalPaidAmountAsync(Guid loanRequestId, CancellationToken cancellationToken = default);

    Task<decimal> GetRemainingAmountAsync(Guid loanRequestId, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<Installment> installments, CancellationToken cancellationToken = default);

    void Update(Installment installment);

    void UpdateRange(IEnumerable<Installment> installments);
}