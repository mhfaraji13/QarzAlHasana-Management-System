using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(m => m.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(m => m.LastName).HasMaxLength(100).IsRequired();
        builder.Property(m => m.NationalCode).HasMaxLength(10).IsRequired();
        builder.Property(m => m.PhoneNumber).HasMaxLength(15).IsRequired();
        builder.Property(m => m.PasswordHash).HasMaxLength(500).IsRequired();
        
        builder.HasIndex(m => m.NationalCode).IsUnique();
        builder.HasIndex(m => m.PhoneNumber).IsUnique();
        
        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}