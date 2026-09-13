using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.API.Contracts.SupportTickets;

public class AddSupportMessageRequest
{
    public string Content { get; set; } = string.Empty;
    public SenderType SenderType { get; set; }
}