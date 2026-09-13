namespace QarzAlHasana.API.Contracts.MembershipPayments;

public class RejectMembershipPaymentRequest
{
    public string RejectionReason { get; set; } = string.Empty;
}