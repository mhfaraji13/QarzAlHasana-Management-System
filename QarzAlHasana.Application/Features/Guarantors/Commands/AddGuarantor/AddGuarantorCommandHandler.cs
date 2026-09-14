using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.Guarantors.Commands.AddGuarantor;

public class AddGuarantorCommandHandler : IRequestHandler<AddGuarantorCommand>
{
    private readonly ILoanRequestRepository _loanRequestRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public AddGuarantorCommandHandler(
        ILoanRequestRepository loanRequestRepository,
        IMemberRepository memberRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _loanRequestRepository = loanRequestRepository;
        _memberRepository = memberRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        AddGuarantorCommand request,
        CancellationToken cancellationToken)
    {
        var loanRequest = await _loanRequestRepository
            .GetByIdWithGuarantorsAsync(request.LoanRequestId, cancellationToken);

        if (loanRequest is null)
        {
            throw new NotFoundException(nameof(LoanRequest), request.LoanRequestId);
        }

        if (loanRequest.MemberId != _currentUserService.UserId)
        {
            throw new ForbiddenException(
                "Shoma faghat mitavanid baraye vam-e khodetan zamen moarrefi konid.");
        }

        var guarantorMember = await _memberRepository
            .GetByIdAsync(request.GuarantorMemberId, cancellationToken);

        if (guarantorMember is null)
        {
            throw new NotFoundException("Ozv", request.GuarantorMemberId);
        }

        if (!guarantorMember.IsActive)
        {
            throw new BusinessRuleException(
                "GUARANTOR_INACTIVE",
                "Ozv-e gheyr-fa'al nemitavanad zamen bashad.");
        }

        if (!guarantorMember.HasPaidRegistrationFee)
        {
            throw new BusinessRuleException(
                "GUARANTOR_REGISTRATION_FEE_NOT_PAID",
                "Ozv-i ke haghe-e sabt-e nam ra pardakht nakarde nemitavanad zamen bashad.");
        }

        loanRequest.AddGuarantor(request.GuarantorMemberId, request.CommittedAmount);

        _loanRequestRepository.Update(loanRequest);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}