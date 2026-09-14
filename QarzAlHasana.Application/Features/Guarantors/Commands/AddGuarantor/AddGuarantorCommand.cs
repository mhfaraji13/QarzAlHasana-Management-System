using MediatR;

namespace QarzAlHasana.Application.Features.Guarantors.Commands.AddGuarantor;

public record AddGuarantorCommand(
    Guid LoanRequestId,
    Guid GuarantorMemberId,
    decimal CommittedAmount) : IRequest;