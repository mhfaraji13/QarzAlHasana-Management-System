using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.MembershipPayments.Queries.GetPendingPayments;

public class PendingPaymentDto
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string MemberFullName { get; set; } = string.Empty;
    public MembershipPaymentType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int? ForMonth { get; set; }
    public int? ForYear { get; set; }
    public string? ReceiptImageUrl { get; set; }
}