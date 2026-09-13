using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Enums;
using QarzAlHasana.Domain.Exceptions;

namespace QarzAlHasana.Domain.Entities;

public class SupportTicket : BaseEntity
{
    public string Subject { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Open;

    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public ICollection<SupportMessage> Messages { get; set; } = new List<SupportMessage>();
    
    public static SupportTicket Create(Guid memberId, string subject, string content)
    {
        var ticket = new SupportTicket
        {
            MemberId = memberId,
            Subject = subject.Trim(),
            Status = TicketStatus.Open
        };

        ticket.AddMessage(content, SenderType.Member);

        return ticket;
    }

    public void AddMessage(string content, SenderType senderType)
    {
        if (Status == TicketStatus.Closed)
        {
            throw new BusinessRuleException(
                "TICKET_CLOSED",
                "Rooye ticket-e baste-shode nemitavan payam ferestad.");
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new BusinessRuleException(
                "MESSAGE_CONTENT_REQUIRED",
                "Matn-e payam elzami ast.");
        }

        Messages.Add(new SupportMessage
        {
            Content = content.Trim(),
            SenderType = senderType,
            SupportTicketId = Id
        });

        if (senderType == SenderType.Admin && Status == TicketStatus.Open)
        {
            Status = TicketStatus.InProgress;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        if (Status == TicketStatus.Closed)
        {
            throw new BusinessRuleException(
                "TICKET_ALREADY_CLOSED",
                "In ticket ghablan baste shode ast.");
        }

        Status = TicketStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }
}

