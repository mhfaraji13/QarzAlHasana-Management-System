namespace QarzAlHasana.Application.Features.Guarantors.Queries.GetPendingGuaranteeRequests;

public class PendingGuaranteeRequestDto
{
    public Guid GuarantorId { get; set; }
    public Guid LoanRequestId { get; set; }
    public string BorrowerFullName { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public int InstallmentCount { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal CommittedAmount { get; set; }
    public DateTime RequestDate { get; set; }
}