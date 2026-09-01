using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Infrastructure.Persistence.Configurations;

public class FundSettingsConfiguration : IEntityTypeConfiguration<FundSettings>
{
    public void Configure(EntityTypeBuilder<FundSettings> builder)
    {
        builder.Property(f => f.MonthlyMembershipFee).HasColumnType("decimal(18,2)");
        builder.Property(f => f.RegistrationFee).HasColumnType("decimal(18,2)");
    }
}