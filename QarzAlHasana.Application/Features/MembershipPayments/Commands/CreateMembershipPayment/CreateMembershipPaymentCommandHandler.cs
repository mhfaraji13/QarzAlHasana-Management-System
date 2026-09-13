using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;
using QarzAlHasana.Domain.Exceptions;

namespace QarzAlHasana.Application.Features.MembershipPayments.Commands.CreateMembershipPayment;

public class CreateMembershipPaymentCommandHandler
    : IRequestHandler<CreateMembershipPaymentCommand, Guid>
{
    private readonly IMembershipPaymentRepository _membershipPaymentRepository;
    private readonly IFundSettingsRepository _fundSettingsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateMembershipPaymentCommandHandler(
        IMembershipPaymentRepository membershipPaymentRepository,
        IFundSettingsRepository fundSettingsRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _membershipPaymentRepository = membershipPaymentRepository;
        _fundSettingsRepository = fundSettingsRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateMembershipPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var memberId = _currentUserService.UserId!.Value;
        decimal amount;

        if (request.Type == MembershipPaymentType.Registration)
        {
            var existing = await _membershipPaymentRepository
                .GetRegistrationPaymentAsync(memberId, cancellationToken);

            if (existing is not null)
            {
                throw new BusinessRuleException(
                    "REGISTRATION_ALREADY_SUBMITTED",
                    "Baraye in ozv ghablan pardakht-e sabt-e nam sabt shode ast.");
            }

            amount = await _fundSettingsRepository
                .GetRegistrationFeeAsync(cancellationToken);
        }
        else
        {
            if (request.ForYear is null || request.ForMonth is null)
            {
                throw new BusinessRuleException(
                    "MONTHLY_PERIOD_REQUIRED",
                    "Baraye pardakht-e mahane، mah va sal elzami ast.");
            }

            var alreadyPaid = await _membershipPaymentRepository.HasPaidForMonthAsync(
                memberId,
                request.ForYear.Value,
                request.ForMonth.Value,
                cancellationToken);

            if (alreadyPaid)
            {
                throw new BusinessRuleException(
                    "MONTH_ALREADY_PAID",
                    $"Baraye {request.ForMonth}/{request.ForYear} ghablan pardakht sabt shode ast.");
            }

            amount = await _fundSettingsRepository
                .GetMonthlyMembershipFeeAsync(cancellationToken);
        }

        var payment = MembershipPayment.Create(
            memberId,
            request.Type,
            amount,
            DateTime.UtcNow,
            request.ForMonth,
            request.ForYear,
            request.ReceiptImageUrl);

        await _membershipPaymentRepository.AddAsync(payment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return payment.Id;
    }
}