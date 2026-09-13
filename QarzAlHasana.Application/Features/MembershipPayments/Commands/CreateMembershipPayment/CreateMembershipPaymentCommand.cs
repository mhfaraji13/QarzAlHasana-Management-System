using MediatR;
using QarzAlHasana.Domain.Enums;

namespace QarzAlHasana.Application.Features.MembershipPayments.Commands.CreateMembershipPayment;

public record CreateMembershipPaymentCommand(
    MembershipPaymentType Type,
    int? ForMonth,
    int? ForYear,
    string? ReceiptImageUrl) : IRequest<Guid>;