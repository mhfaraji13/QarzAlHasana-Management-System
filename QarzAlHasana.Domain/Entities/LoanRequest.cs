using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Enums;
using QarzAlHasana.Domain.Exceptions;

namespace QarzAlHasana.Domain.Entities;

public class LoanRequest : BaseEntity
{
    public decimal Amount { get; set; }

    public int InstallmentCount { get; set; }

    public string Description { get; set; } = string.Empty;

    public LoanStatus Status { get; set; } = LoanStatus.Pending;

    public DateTime RequestDate { get; set; }

    public DateTime? ReviewedDate { get; set; }

    public string? RejectionReason { get; set; }

    public Guid MemberId { get; set; }

    public Member Member { get; set; } = null!;

    public ICollection<Installment> Installments { get; set; } = new List<Installment>();

    public ICollection<Guarantor> Guarantors { get; set; } = new List<Guarantor>();

    /// <summary>
    /// Taeed-e darkhast-e vam. Faghat vam-e dar entezar (Pending) ghabel-e taeed ast.
    /// </summary>
    public void Approve()
    {
        if (Status != LoanStatus.Pending)
        {
            throw new BusinessRuleException(
                "LOAN_NOT_PENDING",
                $"Faghat darkhast-e vam-i ke dar vaziat-e Pending ast ghabel-e taeed mibashad. Vaziat-e feli: {Status}");
        }

        Status = LoanStatus.Approved;
        ReviewedDate = DateTime.UtcNow;
        RejectionReason = null;
        UpdatedAt = DateTime.UtcNow;
    }
}