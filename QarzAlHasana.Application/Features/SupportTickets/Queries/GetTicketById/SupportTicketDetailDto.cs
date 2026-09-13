using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetTicketById;

public class SupportTicketDetailDto
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid MemberId { get; set; }
    public string MemberFullName { get; set; } = string.Empty;

    public List<SupportMessageDto> Messages { get; set; } = new();
}