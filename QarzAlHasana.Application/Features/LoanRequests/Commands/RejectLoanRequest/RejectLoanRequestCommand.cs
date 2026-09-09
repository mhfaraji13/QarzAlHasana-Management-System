using MediatR;

namespace QarzAlHasana.Application.Features.LoanRequests.Commands.RejectLoanRequest;

public record RejectLoanRequestCommand ( 
    Guid LoanRequestId,
    string RejectionReason):IRequest ;