using QarzAlHasana.Domain.Common;

namespace QarzAlHasana.Domain.Entities;

public class FundSettings : BaseEntity
{
    public decimal MonthlyMembershipFee { get; set; }
    public decimal RegistrationFee { get; set; }
}
