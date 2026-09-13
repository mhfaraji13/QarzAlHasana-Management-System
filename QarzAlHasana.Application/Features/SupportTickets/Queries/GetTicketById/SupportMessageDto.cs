using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.SupportTickets.Queries.GetTicketById;

public class SupportMessageDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public SenderType SenderType { get; set; }
    public DateTime CreatedAt { get; set; }
}