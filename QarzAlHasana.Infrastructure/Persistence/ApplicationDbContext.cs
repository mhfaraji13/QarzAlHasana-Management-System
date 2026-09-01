using Microsoft.EntityFrameworkCore;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<LoanRequest> LoanRequests => Set<LoanRequest>();
    public DbSet<Installment> Installments => Set<Installment>();
    public DbSet<Guarantor> Guarantors => Set<Guarantor>();
    public DbSet<MembershipPayment> MembershipPayments => Set<MembershipPayment>();
    public DbSet<FundSettings> FundSettings => Set<FundSettings>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<SupportMessage> SupportMessages => Set<SupportMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}