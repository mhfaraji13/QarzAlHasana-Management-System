using MediatR;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.CreateLoanRequest;

public record CreateLoanRequestCommand(
    Guid MemberId,
    decimal Amount,
    int InstallmentCount,
    string Description) : IRequest<Guid>;