using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;
using QarzAlHasana.Domain.Exceptions;
using Xunit;

namespace QarzAlHasana.Domain.Tests;

public class MembershipPaymentTests
{
    [Fact]
    public void Confirm_Throws_WhenAlreadyConfirmed()
    {
        // Arrange
        var payment = MembershipPayment.Create(
            Guid.NewGuid(),
            MembershipPaymentType.Registration,
            500000m,
            new DateTime(2026, 1, 10),
            null,
            null,
            null);

        payment.Confirm();

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() => payment.Confirm());
    }

    [Fact]
    public void Create_Throws_WhenMonthlyHasNoPeriod()
    {
        // Act + Assert
        Assert.Throws<BusinessRuleException>(() =>
            MembershipPayment.Create(
                Guid.NewGuid(),
                MembershipPaymentType.Monthly,
                200000m,
                new DateTime(2026, 1, 10),
                null,
                null,
                null));
    }
}