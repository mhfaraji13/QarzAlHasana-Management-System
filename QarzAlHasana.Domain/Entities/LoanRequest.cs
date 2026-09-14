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
    /// Ezafe kardan-e zamen. Faghat rooye vam-e dar entezar mojaz ast.
    /// </summary>
    public void AddGuarantor(Guid guarantorMemberId, decimal committedAmount)
    {
        if (Status != LoanStatus.Pending)
        {
            throw new BusinessRuleException(
                "LOAN_NOT_PENDING",
                $"Faghat be vam-e dar entezar mitavan zamen ezafe kard. Vaziat-e feli: {Status}");
        }

        if (guarantorMemberId == MemberId)
        {
            throw new BusinessRuleException(
                "SELF_GUARANTEE_NOT_ALLOWED",
                "Ozv nemitavanad zamen-e vam-e khodash bashad.");
        }

        if (committedAmount <= 0)
        {
            throw new BusinessRuleException(
                "INVALID_COMMITTED_AMOUNT",
                $"Mablagh-e zemanat bayad bozorgtar az sefr bashad. Meghdar-e feli: {committedAmount}");
        }

        if (Guarantors.Any(g => g.GuarantorMemberId == guarantorMemberId))
        {
            throw new BusinessRuleException(
                "GUARANTOR_ALREADY_ADDED",
                "In ozv ghablan be onvan-e zamen sabt shode ast.");
        }

        var totalCommitted = Guarantors.Sum(g => g.CommittedAmount) + committedAmount;

        if (totalCommitted > Amount)
        {
            throw new BusinessRuleException(
                "COMMITTED_AMOUNT_EXCEEDS_LOAN",
                $"Jam-e mablagh-e zemanat-ha ({totalCommitted}) nemitavanad az mablagh-e vam ({Amount}) bishtar bashad.");
        }

        Guarantors.Add(new Guarantor
        {
            LoanRequestId = Id,
            GuarantorMemberId = guarantorMemberId,
            CommittedAmount = committedAmount,
            IsConfirmed = false
        });
    }

    /// <summary>
    /// Taeed-e zemanat tavassot-e khod-e zamen. Hich kas nemitavanad be ja-ye digari taeed konad.
    /// </summary>
    public void ConfirmGuarantor(Guid guarantorId, Guid confirmingMemberId, DateTime confirmedDate)
    {
        if (Status != LoanStatus.Pending)
        {
            throw new BusinessRuleException(
                "LOAN_NOT_PENDING",
                $"Faghat zemanat-e vam-e dar entezar ghabel-e taeed ast. Vaziat-e feli: {Status}");
        }

        var guarantor = Guarantors.FirstOrDefault(g => g.Id == guarantorId);

        if (guarantor is null)
        {
            throw new BusinessRuleException(
                "GUARANTOR_NOT_FOUND",
                "Zamen-i ba in shenase baraye in vam vojood nadarad.");
        }

        if (guarantor.GuarantorMemberId != confirmingMemberId)
        {
            throw new BusinessRuleException(
                "NOT_YOUR_GUARANTEE",
                "Faghat khod-e zamen mitavanad zemanat ra taeed konad.");
        }

        guarantor.Confirm(confirmedDate);

        UpdatedAt = confirmedDate;
    }

    /// <summary>
    /// Taeed-e darkhast-e vam. Faghat vam-e dar entezar (Pending) ba zamen-e taeed-shode ghabel-e taeed ast.
    /// Ba taeed shodan, jadval-e aghsat be soorat-e khodkar sakhte mishavad.
    /// </summary>
    public void Approve()
    {
        if (Status != LoanStatus.Pending)
        {
            throw new BusinessRuleException(
                "LOAN_NOT_PENDING",
                $"Faghat darkhast-e vam-i ke dar vaziat-e Pending ast ghabel-e taeed mibashad. Vaziat-e feli: {Status}");
        }

        if (!Guarantors.Any())
        {
            throw new BusinessRuleException(
                "GUARANTOR_REQUIRED",
                "Vam bedoon-e zamen ghabel-e taeed nist.");
        }

        if (Guarantors.Any(g => !g.IsConfirmed))
        {
            throw new BusinessRuleException(
                "GUARANTEE_NOT_CONFIRMED",
                "Hame-ye zamen-ha bayad zemanat-e khod ra taeed karde bashand.");
        }

        var approvalDate = DateTime.UtcNow;

        Status = LoanStatus.Approved;
        ReviewedDate = approvalDate;
        RejectionReason = null;
        UpdatedAt = approvalDate;

        GenerateInstallments(approvalDate);
    }

    /// <summary>
    /// Sakhtan-e jadval-e aghsat. Mablagh-e paye be paein gerd mishavad va
    /// baghimande be ghest-e akhar ezafe mishavad, ta jam-e aghsat daghighan barabar-e Amount bashad.
    /// </summary>
    private void GenerateInstallments(DateTime approvalDate)
    {
        if (InstallmentCount <= 0)
        {
            throw new BusinessRuleException(
                "INVALID_INSTALLMENT_COUNT",
                $"Tedad-e aghsat bayad bozorgtar az sefr bashad. Meghdar-e feli: {InstallmentCount}");
        }

        if (Amount <= 0)
        {
            throw new BusinessRuleException(
                "INVALID_LOAN_AMOUNT",
                $"Mablagh-e vam bayad bozorgtar az sefr bashad. Meghdar-e feli: {Amount}");
        }

        var baseAmount = Math.Floor(Amount / InstallmentCount * 100m) / 100m;
        var lastAmount = Amount - baseAmount * (InstallmentCount - 1);

        for (var i = 1; i <= InstallmentCount; i++)
        {
            Installments.Add(new Installment
            {
                CreatedAt = approvalDate,
                IsDeleted = false,
                LoanRequestId = Id,
                InstallmentNumber = i,
                Amount = i == InstallmentCount ? lastAmount : baseAmount,
                DueDate = approvalDate.AddMonths(i),
                Status = InstallmentStatus.Unpaid
            });
        }
    }

    public void Reject(string rejectionReason)
    {
        if (Status != LoanStatus.Pending)
            throw new BusinessRuleException(
                "LOAN_NOT_PENDING",
                "Only pending loan requests can be rejected.");

        if (string.IsNullOrWhiteSpace(rejectionReason))
            throw new BusinessRuleException(
                "REJECTION_REASON_REQUIRED",
                "Rejection reason is required.");

        Status = LoanStatus.Rejected;
        RejectionReason = rejectionReason.Trim();
        ReviewedDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void PayInstallment(Guid installmentId, decimal amountPaid, DateTime paymentDate)
    {
        if (Status != LoanStatus.Approved)
        {
            throw new BusinessRuleException(
                "LOAN_NOT_APPROVED",
                $"Faghat baraye vam-e taeed-shode mitavan ghest pardakht kard. Vaziat-e feli: {Status}");
        }

        var nextInstallment = Installments
            .Where(i => i.Status == InstallmentStatus.Unpaid)
            .OrderBy(i => i.InstallmentNumber)
            .FirstOrDefault();

        if (nextInstallment is null)
        {
            throw new BusinessRuleException(
                "NO_UNPAID_INSTALLMENTS",
                "Hich ghest-e pardakht-nashode-i baraye in vam vojood nadarad.");
        }

        if (nextInstallment.Id != installmentId)
        {
            throw new BusinessRuleException(
                "INSTALLMENT_OUT_OF_ORDER",
                $"Aghsat bayad be tartib pardakht shavand. Ghest-e ba'di, shomare {nextInstallment.InstallmentNumber} ast.");
        }

        nextInstallment.Pay(paymentDate, amountPaid);

        if (Installments.All(i => i.Status == InstallmentStatus.Paid))
        {
            Status = LoanStatus.Paid;
        }

        UpdatedAt = paymentDate;
    }
}