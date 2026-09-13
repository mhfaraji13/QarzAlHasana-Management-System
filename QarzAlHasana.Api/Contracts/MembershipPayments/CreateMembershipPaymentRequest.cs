using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.API.Contracts.MembershipPayments;

public class CreateMembershipPaymentRequest
{
    public MembershipPaymentType Type { get; set; }
    public int? ForMonth { get; set; }
    public int? ForYear { get; set; }
    public string? ReceiptImageUrl { get; set; }
}