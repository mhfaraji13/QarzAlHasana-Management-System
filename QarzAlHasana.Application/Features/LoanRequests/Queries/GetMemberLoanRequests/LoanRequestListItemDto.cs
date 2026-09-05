namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetMemberLoanRequests;

public record LoanRequestListItemDto(
    Guid Id,
    decimal Amount,
    int InstallmentCount,
    string Description,
    string Status,
    DateTime RequestDate,
    DateTime? ReviewedDate,
    string? RejectionReason);
    