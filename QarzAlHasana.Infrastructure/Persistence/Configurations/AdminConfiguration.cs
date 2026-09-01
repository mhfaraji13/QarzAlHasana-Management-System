using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Infrastructure.Persistence.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.Property(a => a.FullName).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Email).HasMaxLength(200).IsRequired();
        builder.Property(a => a.PhoneNumber).HasMaxLength(15).IsRequired();
        builder.Property(a => a.PasswordHash).HasMaxLength(500).IsRequired();
        builder.Property(a => a.TwoFactorSecret).HasMaxLength(500);

        builder.HasIndex(a => a.Email).IsUnique();

        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}