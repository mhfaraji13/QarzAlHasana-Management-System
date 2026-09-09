using MediatR;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.RejectLoanRequest;

public class RejectLoanRequestCommandHandler : IRequestHandler<RejectLoanRequestCommand>
{
    private readonly ILoanRequestRepository _loanRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectLoanRequestCommandHandler(
        ILoanRequestRepository loanRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _loanRequestRepository = loanRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        RejectLoanRequestCommand request,
        CancellationToken cancellationToken)
    {
        var loanRequest = await _loanRequestRepository
            .GetByIdAsync(request.LoanRequestId, cancellationToken);

        if (loanRequest is null)
            throw new NotFoundException(nameof(LoanRequest), request.LoanRequestId);

        loanRequest.Reject(request.RejectionReason);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}