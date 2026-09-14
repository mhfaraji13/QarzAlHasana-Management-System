using MediatR;

namespace QarzAlHasana.Application.Features.Guarantors.Commands.ConfirmGuarantor;

public record ConfirmGuarantorCommand(
    Guid LoanRequestId,
    Guid GuarantorId) : IRequest;