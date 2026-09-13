using MediatR;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.MembershipPayments.Commands.CreateMembershipPayment;

public record CreateMembershipPaymentCommand(
    Guid MemberId,
    MembershipPaymentType Type,
    int? ForMonth,
    int? ForYear,
    string? ReceiptImageUrl) : IRequest<Guid>;