using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.MembershipPayments.Commands.RejectMembershipPayment;

public class RejectMembershipPaymentCommandHandler
    : IRequestHandler<RejectMembershipPaymentCommand>
{
    private readonly IMembershipPaymentRepository _membershipPaymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectMembershipPaymentCommandHandler(
        IMembershipPaymentRepository membershipPaymentRepository,
        IUnitOfWork unitOfWork)
    {
        _membershipPaymentRepository = membershipPaymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        RejectMembershipPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _membershipPaymentRepository
            .GetByIdAsync(request.PaymentId, cancellationToken);

        if (payment is null)
        {
            throw new NotFoundException(
                $"Pardakht ba shenase {request.PaymentId} peyda nashod.");
        }

        payment.Reject(request.RejectionReason);

        _membershipPaymentRepository.Update(payment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}