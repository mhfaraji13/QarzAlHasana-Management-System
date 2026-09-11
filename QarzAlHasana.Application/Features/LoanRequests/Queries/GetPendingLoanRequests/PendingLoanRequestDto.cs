namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetPendingLoanRequests;

public class PendingLoanRequestDto
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string MemberFullName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int InstallmentCount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
}