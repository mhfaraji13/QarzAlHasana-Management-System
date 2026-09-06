using MediatR;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.ApproveLoanRequest;

public sealed record ApproveLoanRequestCommand(Guid LoanRequestId) : IRequest<Unit>;