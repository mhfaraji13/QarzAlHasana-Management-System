using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Infrastructure.Persistence.Configurations;

public class GuarantorConfiguration : IEntityTypeConfiguration<Guarantor>
{
    public void Configure(EntityTypeBuilder<Guarantor> builder)
    {
        builder.Property(g => g.CommittedAmount).HasColumnType("decimal(18,2)");
        
        
        builder.HasOne(g => g.LoanRequest)
            .WithMany(l => l.Guarantors)
            .HasForeignKey(g => g.LoanRequestId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(g => g.GuarantorMember)
            .WithMany(m => m.GuarantorFor)
            .HasForeignKey(g => g.GuarantorMemberId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasQueryFilter(g => !g.IsDeleted);
        
    }
}