using QarzAlHasana.Domain.Entities;
using Xunit;

namespace QarzAlHasana.Domain.Tests;

public class InstallmentPenaltyTests
{
    [Fact]
    public void DaysLate_ReturnsZero_WhenPaidOnDueDate()
    {
        // Arrange
        var dueDate = new DateTime(2026, 3, 10);

        var installment = new Installment
        {
            Amount = 1000m,
            DueDate = dueDate
        };

        // Act
        var result = installment.DaysLate(dueDate);

        // Assert
        Assert.Equal(0, result);
    }


    [Fact]
    public void CalculatePenalty_StaysAtThreePercent_WhenElevenDaysLate()
    {
        var dueDate = new DateTime(2026, 1, 10);

        var installment = new Installment
        {
            Amount = 1000m,
            DueDate = dueDate
        };
        
        var result = installment.CalculatePenalty(dueDate.AddDays(1000));
        
        Assert.Equal(480m, result);
    }
}