using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Domain.Entities;

public class Installment : BaseEntity
{
    public int InstallmentNumber { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }

    public InstallmentStatus Status { get; set; } = InstallmentStatus.Unpaid;

    public Guid LoanRequestId { get; set; }
    public LoanRequest LoanRequest { get; set; } = null!;
}