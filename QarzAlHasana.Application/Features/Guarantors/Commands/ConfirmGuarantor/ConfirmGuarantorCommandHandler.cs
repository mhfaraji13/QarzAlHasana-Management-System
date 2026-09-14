using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.Guarantors.Commands.ConfirmGuarantor;

public class ConfirmGuarantorCommandHandler : IRequestHandler<ConfirmGuarantorCommand>
{
    private readonly ILoanRequestRepository _loanRequestRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmGuarantorCommandHandler(
        ILoanRequestRepository loanRequestRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _loanRequestRepository = loanRequestRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ConfirmGuarantorCommand request,
        CancellationToken cancellationToken)
    {
        var loanRequest = await _loanRequestRepository
            .GetByIdWithGuarantorsAsync(request.LoanRequestId, cancellationToken);

        if (loanRequest is null)
        {
            throw new NotFoundException(nameof(LoanRequest), request.LoanRequestId);
        }

        var memberId = _currentUserService.UserId!.Value;

        loanRequest.ConfirmGuarantor(request.GuarantorId, memberId, DateTime.UtcNow);

        _loanRequestRepository.Update(loanRequest);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}