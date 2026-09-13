using Microsoft.EntityFrameworkCore;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Application.Features.SupportTickets.Queries.GetMemberTickets;
using QarzAlHasana.Application.Features.SupportTickets.Queries.GetOpenTickets;
using QarzAlHasana.Application.Features.SupportTickets.Queries.GetTicketById;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Infrastructure.Persistence.Repositories;

public class SupportTicketRepository : ISupportTicketRepository
{
    private readonly ApplicationDbContext _context;

    public SupportTicketRepository(ApplicationDbContext context)
    {
        _context = context;
    }

   public async Task<SupportTicket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<SupportTicket?> GetByIdWithMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .Include(t => t.Member)
            .Include(t => t.Messages.OrderBy(m => m.CreatedAt))
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<SupportTicket>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .AsNoTracking()
            .Where(t => t.MemberId == memberId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SupportTicket>> GetByStatusAsync(TicketStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .AsNoTracking()
            .Include(t => t.Member)
            .Where(t => t.Status == status)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SupportTicket>> GetAllForAdminAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .AsNoTracking()
            .Include(t => t.Member)
            .OrderBy(t => t.Status)
            .ThenByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    public async Task<List<MemberTicketDto>> GetTicketsByMemberAsync(
        Guid memberId,
        CancellationToken cancellationToken)
    {
        return await _context.SupportTickets
            .AsNoTracking()
            .Where(t => t.MemberId == memberId)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new MemberTicketDto
            {
                Id = t.Id,
                Subject = t.Subject,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                MessageCount = t.Messages.Count
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<SupportTicketDetailDto?> GetTicketDetailAsync(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        return await _context.SupportTickets
            .AsNoTracking()
            .Where(t => t.Id == ticketId)
            .Select(t => new SupportTicketDetailDto
            {
                Id = t.Id,
                Subject = t.Subject,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                MemberId = t.MemberId,
                MemberFullName = t.Member.FirstName + " " + t.Member.LastName,
                Messages = t.Messages
                    .OrderBy(m => m.CreatedAt)
                    .Select(m => new SupportMessageDto
                    {
                        Id = m.Id,
                        Content = m.Content,
                        SenderType = m.SenderType,
                        CreatedAt = m.CreatedAt
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetOpenTicketCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .CountAsync(t => t.Status == TicketStatus.Open, cancellationToken);
    }

    public async Task<bool> BelongsToMemberAsync(Guid ticketId, Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .AnyAsync(t => t.Id == ticketId && t.MemberId == memberId, cancellationToken);
    }

    public async Task AddAsync(SupportTicket ticket, CancellationToken cancellationToken = default)
    {
        await _context.SupportTickets.AddAsync(ticket, cancellationToken);
    }
    public async Task<List<OpenTicketDto>> GetOpenTicketsAsync(
        CancellationToken cancellationToken)
    {
        return await _context.SupportTickets
            .AsNoTracking()
            .Where(t => t.Status != TicketStatus.Closed)
            .Select(t => new OpenTicketDto
            {
                Id = t.Id,
                Subject = t.Subject,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                MemberId = t.MemberId,
                MemberFullName = t.Member.FirstName + " " + t.Member.LastName,
                MessageCount = t.Messages.Count,
                LastMessageAt = t.Messages.Max(m => m.CreatedAt)
            })
            .OrderBy(t => t.LastMessageAt)
            .ToListAsync(cancellationToken);
    }

    public void Update(SupportTicket ticket)
    {
        _context.SupportTickets.Update(ticket);
    }

    public void Remove(SupportTicket ticket)
    {
        ticket.IsDeleted = true;
        _context.SupportTickets.Update(ticket);
    }
}