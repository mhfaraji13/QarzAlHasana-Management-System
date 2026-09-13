using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetOpenTickets;

public class OpenTicketDto
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid MemberId { get; set; }
    public string MemberFullName { get; set; } = string.Empty;
    public int MessageCount { get; set; }
    public DateTime LastMessageAt { get; set; }
}