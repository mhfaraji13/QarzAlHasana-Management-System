using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanRequestById;

public class GetLoanRequestByIdQueryHandler
    : IRequestHandler<GetLoanRequestByIdQuery, LoanRequestDetailDto>
{
    private readonly ILoanRequestRepository _loanRequestRepository;

    public GetLoanRequestByIdQueryHandler(ILoanRequestRepository loanRequestRepository)
    {
        _loanRequestRepository = loanRequestRepository;
    }

    public async Task<LoanRequestDetailDto> Handle(
        GetLoanRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _loanRequestRepository.GetDetailByIdAsync(request.Id, cancellationToken);

        if (result is null)
        {
            throw new NotFoundException(
                $"Darkhâst-e vâm bâ shenâse {request.Id} peydâ nashod.");
        }

        return result;
    }
}