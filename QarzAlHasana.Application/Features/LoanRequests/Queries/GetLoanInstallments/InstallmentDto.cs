using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanInstallments;

public sealed record InstallmentDto(
    Guid Id,
    int InstallmentNumber,
    decimal Amount,
    DateTime DueDate,
    InstallmentStatus Status,
    DateTime? PaidDate,
    int DaysLate,
    decimal PenaltyAmount,
    decimal TotalDue,
    bool IsOverdue,
    bool WasPaidLate);