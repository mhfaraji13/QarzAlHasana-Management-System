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

    Task AddAsync(SupportTicket ticket, CancellationToken cancellationToken = default);

    void Update(SupportTicket ticket);

    void Remove(SupportTicket ticket);
}