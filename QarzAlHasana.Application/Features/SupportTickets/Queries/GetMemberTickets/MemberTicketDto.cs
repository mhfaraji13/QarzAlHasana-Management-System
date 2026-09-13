using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetMemberTickets;

public class MemberTicketDto
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int MessageCount { get; set; }
}