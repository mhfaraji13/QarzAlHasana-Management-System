using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Application.Features.LoanRequests.Commands.CreateLoanRequest;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;
using QarzAlHasanaSystem.Application.Common.Exceptions;



namespace QarzAlHasanaSystem.Application.Features.LoanRequests.Commands.CreateLoanRequest;

public class CreateLoanRequestCommandHandler
    : IRequestHandler<CreateLoanRequestCommand, Guid>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILoanRequestRepository _loanRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLoanRequestCommandHandler(
        IMemberRepository memberRepository,
        ILoanRequestRepository loanRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _loanRequestRepository = loanRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateLoanRequestCommand request,
        CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken);

        if (member is null)
            throw new NotFoundException("Ozv", request.MemberId);

        if (!member.IsActive)
            throw new BusinessRuleException(
                "MEMBER_INACTIVE",
                "Hesab-e in ozv gheyr-fa'al ast.");

        if (!member.HasPaidRegistrationFee)
            throw new BusinessRuleException(
                "REGISTRATION_FEE_NOT_PAID",
                "Ta zamani ke haghe-e sabt-e nam pardakht nashode, emkan-e sabt-e darkhast-e vam vojood nadarad.");

        var hasActiveLoan = await _loanRequestRepository.HasActiveLoanAsync(request.MemberId, cancellationToken);

        if (hasActiveLoan)
            throw new BusinessRuleException(
                "ACTIVE_LOAN_EXISTS",
                "In ozv yek vam-e faal darad.");

        var loanRequest = new LoanRequest
        {
            MemberId = request.MemberId,
            Amount = request.Amount,
            InstallmentCount = request.InstallmentCount,
            Description = request.Description,
            Status = LoanStatus.Pending,
            RequestDate = DateTime.UtcNow
        };

        await _loanRequestRepository.AddAsync(loanRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return loanRequest.Id;
    }
}