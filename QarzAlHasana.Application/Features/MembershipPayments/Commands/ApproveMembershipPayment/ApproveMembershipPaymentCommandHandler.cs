using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasana.Domain.Enums;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.MembershipPayments.Commands.ApproveMembershipPayment;

public class ApproveMembershipPaymentCommandHandler
    : IRequestHandler<ApproveMembershipPaymentCommand>
{
    private readonly IMembershipPaymentRepository _membershipPaymentRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveMembershipPaymentCommandHandler(
        IMembershipPaymentRepository membershipPaymentRepository,
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork)
    {
        _membershipPaymentRepository = membershipPaymentRepository;
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ApproveMembershipPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _membershipPaymentRepository
            .GetByIdAsync(request.PaymentId, cancellationToken);

        if (payment is null)
        {
            throw new NotFoundException(
                $"Pardakht ba shenase {request.PaymentId} peyda nashod.");
        }

        payment.Confirm();

        if (payment.Type == MembershipPaymentType.Registration)
        {
            var member = await _memberRepository
                .GetByIdAsync(payment.MemberId, cancellationToken);

            if (member is null)
            {
                throw new NotFoundException(
                    $"Ozv ba shenase {payment.MemberId} peyda nashod.");
            }

            member.HasPaidRegistrationFee = true;
            member.UpdatedAt = DateTime.UtcNow;

            _memberRepository.Update(member);
        }

        _membershipPaymentRepository.Update(payment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return;
    }
}