using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Infrastructure.Persistence.Configurations;

public class LoanRequestConfiguration : IEntityTypeConfiguration<LoanRequest>
{
    public void Configure(EntityTypeBuilder<LoanRequest> builder)
    {
        builder.Property(l => l.Amount).HasColumnType("decimal(18,2)");
        builder.Property(l => l.Description).HasMaxLength(1000);
        builder.Property(l => l.RejectionReason).HasMaxLength(1000);
        
        builder.HasOne(l => l.Member)
            .WithMany(m => m.LoanRequests)
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}