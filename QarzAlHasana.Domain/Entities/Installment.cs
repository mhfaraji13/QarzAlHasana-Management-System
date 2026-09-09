using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Enums;
using QarzAlHasana.Domain.Exceptions;

namespace QarzAlHasana.Domain.Entities;

public class Installment : BaseEntity
{
    /// <summary>Tedad-e rooz-haye har pele-ye jarime.</summary>
    public const int PenaltyBlockDays = 6;

    /// <summary>Darsad-e jarime be ezaye har pele-ye kamel-e 6 rooze.</summary>
    public const decimal PenaltyRatePerBlock = 0.03m;

    /// <summary>Saghf-e jarime, harchand rooz ke ta'khir bashad.</summary>
    public const decimal MaxPenaltyRate = 0.48m;

    public int InstallmentNumber { get; set; }

    public decimal Amount { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? PaidDate { get; set; }

    /// <summary>Jarime-ye freeze-shode dar lahze-ye pardakht. Ta ghabl az pardakht null ast.</summary>
    public decimal? PenaltyAmount { get; set; }

    /// <summary>Kol-e mablagh-e daryaft-shode (ghest + jarime). Ta ghabl az pardakht null ast.</summary>
    public decimal? PaidAmount { get; set; }

    public InstallmentStatus Status { get; set; } = InstallmentStatus.Unpaid;

    public Guid LoanRequestId { get; set; }

    public LoanRequest LoanRequest { get; set; } = null!;

    /// <summary>Ghest-e pardakht-shode-i ke jarime khorde bood.</summary>
    public bool WasPaidLate => PenaltyAmount > 0m;

    /// <summary>Tedad-e rooz-haye ta'khir ta tarikh-e dade-shode. Age deyr nashode bashad, sefr.</summary>
    public int DaysLate(DateTime asOf)
    {
        if (asOf.Date <= DueDate.Date)
        {
            return 0;
        }

        return (asOf.Date - DueDate.Date).Days;
    }

    /// <summary>Tedad-e pele-haye kamel-e 6 rooze-i ke gozashte ast.</summary>
    public int CompletedPenaltyBlocks(DateTime asOf)
    {
        return DaysLate(asOf) / PenaltyBlockDays;
    }

    /// <summary>Ghest-e pardakht-nashode-i ke tarikhash gozashte ast.</summary>
    public bool IsOverdue(DateTime asOf)
    {
        return Status == InstallmentStatus.Unpaid && DaysLate(asOf) > 0;
    }

    /// <summary>
    /// Jarime ta tarikh-e dade-shode. Age ghest pardakht shode bashad,
    /// hamoon jarime-ye freeze-shode bar migardad va digar taghyir nemikonad.
    /// </summary>
    public decimal CalculatePenalty(DateTime asOf)
    {
        if (Status == InstallmentStatus.Paid)
        {
            return PenaltyAmount ?? 0m;
        }

        var blocks = CompletedPenaltyBlocks(asOf);

        if (blocks == 0)
        {
            return 0m;
        }

        var rate = Math.Min(PenaltyRatePerBlock * blocks, MaxPenaltyRate);

        return Math.Floor(Amount * rate * 100m) / 100m;
    }

    /// <summary>Kol-e mablaghi ke bayad yekja pardakht shavad: ghest + jarime.</summary>
    public decimal TotalDue(DateTime asOf)
    {
        if (Status == InstallmentStatus.Paid)
        {
            return 0m;
        }

        return Amount + CalculatePenalty(asOf);
    }

    /// <summary>
    /// Pardakht-e ghest. Faghat az tarigh-e LoanRequest ghabel-e seda zadan ast.
    /// Mablagh bayad daghighan barabar-e ghest + jarime bashad; pardakht-e joda ejaze nadarad.
    /// </summary>
    internal void Pay(DateTime paymentDate, decimal amountPaid)
    {
        if (Status == InstallmentStatus.Paid)
        {
            throw new BusinessRuleException(
                "INSTALLMENT_ALREADY_PAID",
                $"Ghest-e shomare {InstallmentNumber} ghablan pardakht shode ast.");
        }

        var penalty = CalculatePenalty(paymentDate);
        var totalDue = Amount + penalty;

        if (amountPaid != totalDue)
        {
            throw new BusinessRuleException(
                "PAYMENT_AMOUNT_MISMATCH",
                $"Mablagh-e pardakhti bayad daghighan {totalDue} bashad (ghest: {Amount} + jarime: {penalty}). Mablagh-e ersali: {amountPaid}");
        }

        Status = InstallmentStatus.Paid;
        PaidDate = paymentDate;
        PenaltyAmount = penalty;
        PaidAmount = totalDue;
        UpdatedAt = paymentDate;
    }
}