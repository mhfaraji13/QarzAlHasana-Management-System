using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Domain.Entities;

public class SupportTicket : BaseEntity
{
    public string Subject { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Open;

    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public ICollection<SupportMessage> Messages { get; set; } = new List<SupportMessage>();
}

