using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanRequestById;

public class LoanRequestDetailDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public int InstallmentCount { get; set; }
    public string Description { get; set; } = string.Empty;
    public LoanStatus Status { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public string? RejectionReason { get; set; }

    public Guid MemberId { get; set; }
    public string MemberFullName { get; set; } = string.Empty;
    public string MemberNationalCode { get; set; } = string.Empty;
    public string MemberPhoneNumber { get; set; } = string.Empty;

    public List<LoanGuarantorDto> Guarantors { get; set; } = new();
}