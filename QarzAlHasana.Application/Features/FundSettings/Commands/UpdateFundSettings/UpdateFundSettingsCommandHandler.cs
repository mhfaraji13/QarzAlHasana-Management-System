using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.FundSettings.Commands.UpdateFundSettings;

public class UpdateFundSettingsCommandHandler
    : IRequestHandler<UpdateFundSettingsCommand>
{
    private readonly IFundSettingsRepository _fundSettingsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFundSettingsCommandHandler(
        IFundSettingsRepository fundSettingsRepository,
        IUnitOfWork unitOfWork)
    {
        _fundSettingsRepository = fundSettingsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateFundSettingsCommand request,
        CancellationToken cancellationToken)
    {
        var settings = await _fundSettingsRepository.GetAsync(cancellationToken);

        if (settings is null)
        {
            throw new NotFoundException(
                "Tanzimat-e sandogh hanooz sabt nashode ast.");
        }

        settings.RegistrationFee = request.RegistrationFee;
        settings.MonthlyMembershipFee = request.MonthlyMembershipFee;
        settings.UpdatedAt = DateTime.UtcNow;

        _fundSettingsRepository.Update(settings);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}