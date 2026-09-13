namespace QarzAlHasana.API.Contracts.SupportTickets;

public class CreateSupportTicketRequest
{
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}