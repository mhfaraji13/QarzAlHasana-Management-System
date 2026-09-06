using MediatR;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.ApproveLoanRequest;

public sealed class ApproveLoanRequestCommandHandler
    : IRequestHandler<ApproveLoanRequestCommand, Unit>
{
    private readonly ILoanRequestRepository _loanRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveLoanRequestCommandHandler(
        ILoanRequestRepository loanRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _loanRequestRepository = loanRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        ApproveLoanRequestCommand request,
        CancellationToken cancellationToken)
    {
        var loanRequest = await _loanRequestRepository
            .GetByIdAsync(request.LoanRequestId, cancellationToken);

        if (loanRequest is null)
        {
            throw new NotFoundException(nameof(LoanRequest), request.LoanRequestId);
        }

        loanRequest.Approve();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}