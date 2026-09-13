using MediatR;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.CreateLoanRequest;

public record CreateLoanRequestCommand(
    decimal Amount,
    int InstallmentCount,
    string Description) : IRequest<Guid>;