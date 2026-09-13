using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Exceptions;

namespace QarzAlHasana.Domain.Entities;

public class Member : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public bool HasPaidRegistrationFee { get; set; } = false;

    public ICollection<LoanRequest> LoanRequests { get; set; } = new List<LoanRequest>();
    public ICollection<Guarantor> GuarantorFor { get; set; } = new List<Guarantor>();
    public ICollection<MembershipPayment> MembershipPayments { get; set; } = new List<MembershipPayment>();
    public void Activate()
    {
        if (IsActive)
        {
            throw new BusinessRuleException(
                "MEMBER_ALREADY_ACTIVE",
                "In ozv ghablan fa'al shode ast.");
        }

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new BusinessRuleException(
                "MEMBER_ALREADY_INACTIVE",
                "In ozv ghablan gheyr-e fa'al shode ast.");
        }

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
    public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();

}

