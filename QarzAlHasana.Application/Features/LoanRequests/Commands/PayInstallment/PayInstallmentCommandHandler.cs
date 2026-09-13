using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.PayInstallment;

public sealed class PayInstallmentCommandHandler
    : IRequestHandler<PayInstallmentCommand, Unit>
{
    private readonly ILoanRequestRepository _loanRequestRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public PayInstallmentCommandHandler(
        ILoanRequestRepository loanRequestRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _loanRequestRepository = loanRequestRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        PayInstallmentCommand request,
        CancellationToken cancellationToken)
    {
        var loanRequest = await _loanRequestRepository
            .GetByIdWithInstallmentsAsync(request.LoanRequestId, cancellationToken);

        if (loanRequest is null)
        {
            throw new NotFoundException(nameof(LoanRequest), request.LoanRequestId);
        }

        if (loanRequest.MemberId != _currentUserService.UserId)
        {
            throw new ForbiddenException(
                "Shoma faghat mitavanid aghsat-e vam-e khodetan ra pardakht konid.");
        }

        loanRequest.PayInstallment(
            request.InstallmentId,
            request.AmountPaid,
            DateTime.UtcNow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}