using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Exceptions;

namespace QarzAlHasana.Domain.Entities;

public class Guarantor : BaseEntity
{
    public decimal CommittedAmount { get; set; }
    public bool IsConfirmed { get; set; } = false;
    public DateTime? ConfirmedDate { get; set; }

    public Guid GuarantorMemberId { get; set; }
    public Member GuarantorMember { get; set; } = null!;

    public Guid LoanRequestId { get; set; }
    public LoanRequest LoanRequest { get; set; } = null!;

    /// <summary>Taeed-e zemanat. Faghat az tarigh-e LoanRequest ghabel-e seda zadan ast.</summary>
    internal void Confirm(DateTime confirmedDate)
    {
        if (IsConfirmed)
        {
            throw new BusinessRuleException(
                "GUARANTEE_ALREADY_CONFIRMED",
                "In zemanat ghablan taeed shode ast.");
        }

        IsConfirmed = true;
        ConfirmedDate = confirmedDate;
        UpdatedAt = confirmedDate;
    }
}