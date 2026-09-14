namespace QarzAlHasana.API.Contracts.Guarantors;

public class AddGuarantorRequest
{
    public Guid GuarantorMemberId { get; set; }
    public decimal CommittedAmount { get; set; }
}