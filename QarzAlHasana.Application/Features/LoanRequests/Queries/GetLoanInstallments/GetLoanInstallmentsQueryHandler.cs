using MediatR;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Enums;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.LoanRequests.Queries.GetLoanInstallments;

public sealed class GetLoanInstallmentsQueryHandler
    : IRequestHandler<GetLoanInstallmentsQuery, LoanInstallmentsDto>
{
    private readonly ILoanRequestRepository _loanRequestRepository;

    public GetLoanInstallmentsQueryHandler(ILoanRequestRepository loanRequestRepository)
    {
        _loanRequestRepository = loanRequestRepository;
    }

    public async Task<LoanInstallmentsDto> Handle(
        GetLoanInstallmentsQuery request,
        CancellationToken cancellationToken)
    {
        var loanRequest = await _loanRequestRepository
            .GetByIdWithInstallmentsAsync(request.LoanRequestId, cancellationToken);

        if (loanRequest is null)
        {
            throw new NotFoundException(nameof(LoanRequest), request.LoanRequestId);
        }

        var asOf = DateTime.UtcNow;

        var installments = loanRequest.Installments
            .OrderBy(i => i.InstallmentNumber)
            .Select(i => new InstallmentDto(
                i.Id,
                i.InstallmentNumber,
                i.Amount,
                i.DueDate,
                i.Status,
                i.PaidDate,
                i.DaysLate(asOf),
                i.CalculatePenalty(asOf),
                i.TotalDue(asOf),
                i.IsOverdue(asOf),
                i.WasPaidLate))
            .ToList();

        var nextInstallment = installments
            .FirstOrDefault(i => i.Status == InstallmentStatus.Unpaid);

        var totalRemaining = installments.Sum(i => i.TotalDue);

        return new LoanInstallmentsDto(
            loanRequest.Id,
            loanRequest.Status,
            asOf,
            nextInstallment,
            totalRemaining,
            installments);
    }
}