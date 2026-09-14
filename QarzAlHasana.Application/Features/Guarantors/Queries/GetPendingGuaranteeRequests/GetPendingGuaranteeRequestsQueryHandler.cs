using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;

namespace QarzAlHasana.Application.Features.Guarantors.Queries.GetPendingGuaranteeRequests;

public class GetPendingGuaranteeRequestsQueryHandler
    : IRequestHandler<GetPendingGuaranteeRequestsQuery, List<PendingGuaranteeRequestDto>>
{
    private readonly IGuarantorRepository _guarantorRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetPendingGuaranteeRequestsQueryHandler(
        IGuarantorRepository guarantorRepository,
        ICurrentUserService currentUserService)
    {
        _guarantorRepository = guarantorRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<PendingGuaranteeRequestDto>> Handle(
        GetPendingGuaranteeRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var memberId = _currentUserService.UserId!.Value;

        return await _guarantorRepository
            .GetPendingForMemberAsync(memberId, cancellationToken);
    }
}