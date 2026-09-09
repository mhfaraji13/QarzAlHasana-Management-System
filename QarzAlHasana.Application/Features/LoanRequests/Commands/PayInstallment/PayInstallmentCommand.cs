using MediatR;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.PayInstallment;

public sealed record PayInstallmentCommand(
    Guid LoanRequestId,
    Guid InstallmentId,
    decimal AmountPaid) : IRequest<Unit>;