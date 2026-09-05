using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Common.Interfaces.Repositories;

public interface IMembershipPaymentRepository
{
    Task<MembershipPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MembershipPayment?> GetByIdWithMemberAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MembershipPayment>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MembershipPayment>> GetByStatusAsync(DepositStatus status, CancellationToken cancellationToken = default);

    Task<MembershipPayment?> GetRegistrationPaymentAsync(Guid memberId, CancellationToken cancellationToken = default);

    Task<bool> HasPaidForMonthAsync(Guid memberId, int year, int month, CancellationToken cancellationToken = default);

    Task<decimal> GetTotalConfirmedAmountAsync(Guid memberId, CancellationToken cancellationToken = default);

    Task<decimal> GetFundTotalBalanceAsync(CancellationToken cancellationToken = default);

    Task<int> GetPendingCountAsync(CancellationToken cancellationToken = default);

    Task AddAsync(MembershipPayment payment, CancellationToken cancellationToken = default);

    void Update(MembershipPayment payment);

    void Remove(MembershipPayment payment);
}