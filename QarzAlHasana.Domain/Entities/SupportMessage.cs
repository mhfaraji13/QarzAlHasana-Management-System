using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Domain.Entities;

public class SupportMessage : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public SenderType SenderType { get; set; } 

    public Guid SupportTicketId { get; set; }
    public SupportTicket SupportTicket { get; set; } = null!;
}