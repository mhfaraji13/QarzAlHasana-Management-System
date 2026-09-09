using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanInstallments;

public sealed record LoanInstallmentsDto(
    Guid LoanRequestId,
    LoanStatus LoanStatus,
    DateTime AsOf,
    InstallmentDto? NextInstallment,
    decimal TotalRemaining,
    IReadOnlyList<InstallmentDto> Installments);