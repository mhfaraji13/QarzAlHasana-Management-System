using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Enums;
using QarzAlHasana.Domain.Exceptions;

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
    public static MembershipPayment Create(
        Guid memberId,
        MembershipPaymentType type,
        decimal amount,
        DateTime paymentDate,
        int? forMonth,
        int? forYear,
        string? receiptImageUrl)
    {
        if (type == MembershipPaymentType.Monthly)
        {
            if (forMonth is null || forYear is null)
            {
                throw new BusinessRuleException(
                    "MONTHLY_PERIOD_REQUIRED",
                    "Baraye pardakht-e mahane، mah va sal elzami ast.");
            }

            if (forMonth is < 1 or > 12)
            {
                throw new BusinessRuleException(
                    "INVALID_MONTH",
                    $"Mah bayad beyn-e 1 ta 12 bashad. Meghdar-e feli: {forMonth}");
            }
        }
        else
        {
            forMonth = null;
            forYear = null;
        }

        return new MembershipPayment
        {
            MemberId = memberId,
            Type = type,
            Amount = amount,
            PaymentDate = paymentDate,
            ForMonth = forMonth,
            ForYear = forYear,
            ReceiptImageUrl = receiptImageUrl,
            Status = DepositStatus.Pending
        };
    }
}