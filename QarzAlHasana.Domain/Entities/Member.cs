using QarzAlHasana.Domain.Common;

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
    public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();

}

