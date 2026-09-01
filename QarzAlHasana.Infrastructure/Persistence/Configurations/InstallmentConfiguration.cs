using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Infrastructure.Persistence.Configurations;

public class InstallmentConfiguration : IEntityTypeConfiguration<Installment>
{
    public void Configure(EntityTypeBuilder<Installment> builder)
    {
        builder.Property(i => i.Amount).HasColumnType("decimal(18,2)");
        builder.HasOne(i => i.LoanRequest)
            .WithMany(l => l.Installments)
            .HasForeignKey(i => i.LoanRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}