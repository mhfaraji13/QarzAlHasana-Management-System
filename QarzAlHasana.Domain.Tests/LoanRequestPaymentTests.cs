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
            Id = Guid.NewGuid(),
            MemberId = Guid.NewGuid(),
            Amount = 1200m,
            InstallmentCount = 12,
            Description = "test"
        };

        var guarantorMemberId = Guid.NewGuid();

        loan.AddGuarantor(guarantorMemberId, 1200m);

        var guarantor = loan.Guarantors.First();
        guarantor.Id = Guid.NewGuid();

        loan.ConfirmGuarantor(guarantor.Id, guarantorMemberId, DateTime.UtcNow);

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

    [Fact]
    public void Approve_Throws_WhenNoGuarantor()
    {
        // Arrange
        var loan = new LoanRequest
        {
            Id = Guid.NewGuid(),
            MemberId = Guid.NewGuid(),
            Amount = 1200m,
            InstallmentCount = 12,
            Description = "test"
        };

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() => loan.Approve());
    }

    [Fact]
    public void Approve_Throws_WhenGuaranteeNotConfirmed()
    {
        // Arrange
        var loan = new LoanRequest
        {
            Id = Guid.NewGuid(),
            MemberId = Guid.NewGuid(),
            Amount = 1200m,
            InstallmentCount = 12,
            Description = "test"
        };

        loan.AddGuarantor(Guid.NewGuid(), 1200m);

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() => loan.Approve());
    }

    [Fact]
    public void AddGuarantor_Throws_WhenMemberGuaranteesOwnLoan()
    {
        // Arrange
        var memberId = Guid.NewGuid();

        var loan = new LoanRequest
        {
            Id = Guid.NewGuid(),
            MemberId = memberId,
            Amount = 1200m,
            InstallmentCount = 12,
            Description = "test"
        };

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() =>
            loan.AddGuarantor(memberId, 1200m));
    }
}