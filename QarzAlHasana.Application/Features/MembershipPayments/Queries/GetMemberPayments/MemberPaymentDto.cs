using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.MembershipPayments.Queries.GetMemberPayments;

public class MemberPaymentDto
{
    public Guid Id { get; set; }
    public MembershipPaymentType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DepositStatus Status { get; set; }
    public int? ForMonth { get; set; }
    public int? ForYear { get; set; }
    public string? ReceiptImageUrl { get; set; }
    public string? RejectionReason { get; set; }
}