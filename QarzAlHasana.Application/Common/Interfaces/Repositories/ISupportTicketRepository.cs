using QarzAlHasana.Application.Features.SupportTickets.Queries.GetMemberTickets;
using QarzAlHasana.Application.Features.SupportTickets.Queries.GetOpenTickets;
using QarzAlHasana.Application.Features.SupportTickets.Queries.GetTicketById;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Common.Interfaces.Repositories;

public interface ISupportTicketRepository
{
    Task<SupportTicket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SupportTicket?> GetByIdWithMessagesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SupportTicket>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SupportTicket>> GetByStatusAsync(TicketStatus status, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SupportTicket>> GetAllForAdminAsync(CancellationToken cancellationToken = default);

    Task<int> GetOpenTicketCountAsync(CancellationToken cancellationToken = default);

    Task<bool> BelongsToMemberAsync(Guid ticketId, Guid memberId, CancellationToken cancellationToken = default);
    Task<List<MemberTicketDto>> GetTicketsByMemberAsync(Guid memberId, CancellationToken cancellationToken);

    Task<SupportTicketDetailDto?> GetTicketDetailAsync(Guid ticketId, CancellationToken cancellationToken);
    Task<List<OpenTicketDto>> GetOpenTicketsAsync(CancellationToken cancellationToken);

    Task AddAsync(SupportTicket ticket, CancellationToken cancellationToken = default);

    void Update(SupportTicket ticket);

    void Remove(SupportTicket ticket);
}