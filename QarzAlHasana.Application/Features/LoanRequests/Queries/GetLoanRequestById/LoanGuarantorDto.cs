namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanRequestById;

public class LoanGuarantorDto
{
    public Guid Id { get; set; }
    public Guid GuarantorMemberId { get; set; }
    public string GuarantorFullName { get; set; } = string.Empty;
    public string GuarantorPhoneNumber { get; set; } = string.Empty;
    public decimal CommittedAmount { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime? ConfirmedDate { get; set; }
}