using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Domain.Entities;

namespace QarzAlHasana.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAdminAsync(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var email = configuration["SeedAdmin:Email"];

        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        var exists = await context.Admins
            .AnyAsync(a => a.Email == email, cancellationToken);

        if (exists)
        {
            return;
        }

        var admin = new Admin
        {
            FullName = configuration["SeedAdmin:FullName"] ?? "Admin",
            Email = email,
            PhoneNumber = configuration["SeedAdmin:PhoneNumber"] ?? string.Empty,
            PasswordHash = passwordHasher.Hash(configuration["SeedAdmin:Password"]!)
        };

        context.Admins.Add(admin);

        await context.SaveChangesAsync(cancellationToken);
    }
}