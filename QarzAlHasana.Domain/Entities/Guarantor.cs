using QarzAlHasana.Domain.Common;

namespace QarzAlHasana.Domain.Entities;

public class Guarantor:BaseEntity
{
    public decimal CommittedAmount { get; set; }
    public bool IsConfirmed { get; set; } = false;
    public DateTime? ConfirmedDate { get; set; }
    
    public Guid GuarantorMemberId { get; set; }
    public Member GuarantorMember { get; set; } = null!;
    
    public Guid LoanRequestId { get; set; }
    public LoanRequest LoanRequest { get; set; } = null!;
}