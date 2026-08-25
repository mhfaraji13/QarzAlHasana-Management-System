using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Domain.Entities;

public class MembershipPayment : BaseEntity
{
    public MembershipPaymentType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? ReceiptImageUrl { get; set; }

    public DepositStatus Status { get; set; } = DepositStatus.Pending;
    
    
    public int? ForMonth { get; set; }
    public int? ForYear { get; set; }

    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;
}