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
}