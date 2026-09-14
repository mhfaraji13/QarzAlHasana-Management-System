using MediatR;

namespace QarzAlHasana.Application.Features.Guarantors.Queries.GetPendingGuaranteeRequests;

public record GetPendingGuaranteeRequestsQuery : IRequest<List<PendingGuaranteeRequestDto>>;