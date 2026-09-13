namespace QarzAlHasana.Application.Features.FundSettings.Queries.GetFundSettings;

public class FundSettingsDto
{
    public Guid Id { get; set; }
    public decimal RegistrationFee { get; set; }
    public decimal MonthlyMembershipFee { get; set; }
    public DateTime? UpdatedAt { get; set; }
}