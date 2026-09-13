using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Common;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanRequestById;

public class GetLoanRequestByIdQueryHandler
    : IRequestHandler<GetLoanRequestByIdQuery, LoanRequestDetailDto>
{
    private readonly ILoanRequestRepository _loanRequestRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetLoanRequestByIdQueryHandler(
        ILoanRequestRepository loanRequestRepository,
        ICurrentUserService currentUserService)
    {
        _loanRequestRepository = loanRequestRepository;
        _currentUserService = currentUserService;
    }

    public async Task<LoanRequestDetailDto> Handle(
        GetLoanRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _loanRequestRepository
            .GetDetailByIdAsync(request.Id, cancellationToken);

        if (result is null)
        {
            throw new NotFoundException(
                $"Darkhast-e vam ba shenase {request.Id} peyda nashod.");
        }

        if (_currentUserService.Role != Roles.Admin &&
            result.MemberId != _currentUserService.UserId)
        {
            throw new ForbiddenException(
                "Shoma be in darkhast-e vam dastresi nadarid.");
        }

        return result;
    }
}