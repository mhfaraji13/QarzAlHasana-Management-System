using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Exceptions;
using Xunit;

namespace QarzAlHasana.Domain.Tests;

public class LoanRequestPaymentTests
{
    [Fact]
    public void PayInstallment_Throws_WhenPaidOutOfOrder()
    {
        // Arrange
        var loan = new LoanRequest
        {
            Amount = 1200m,
            InstallmentCount = 12,
            Description = "test"
        };

        loan.Approve();

        foreach (var installment in loan.Installments)
        {
            installment.Id = Guid.NewGuid();
        }

        var second = loan.Installments.First(i => i.InstallmentNumber == 2);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() =>
            loan.PayInstallment(second.Id, second.Amount, DateTime.UtcNow));
    }
}