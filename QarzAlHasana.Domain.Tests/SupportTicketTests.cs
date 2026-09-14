using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;
using QarzAlHasana.Domain.Exceptions;
using Xunit;

namespace QarzAlHasana.Domain.Tests;

public class SupportTicketTests
{
    [Fact]
    public void AddMessage_Throws_WhenTicketIsClosed()
    {
        // Arrange
        var ticket = SupportTicket.Create(
            Guid.NewGuid(),
            "Moshkel dar pardakht",
            "Fish-e man tayid nashode.");

        ticket.Close();

        // Act + Assert
        Assert.Throws<BusinessRuleException>(() =>
            ticket.AddMessage("Peygiri mikonam.", SenderType.Admin));
    }
}