using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Domain.Entities;

public class LoanRequest : BaseEntity
{
    public decimal Amount { get; set; }
    public int InstallmentCount { get; set; }
    public string Description { get; set; } = string.Empty;

    public LoanStatus Status { get; set; } = LoanStatus.Pending;

    public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedDate { get; set; }
    public string? RejectionReason { get; set; }

    public Guid MemberId  { get; set; }
    public Member Member { get; set; } = null!;

    public ICollection<Installment> Installments { get; set; } = new List<Installment>();
    public ICollection<Guarantor> Guarantors { get; set; } = new List<Guarantor>();
}