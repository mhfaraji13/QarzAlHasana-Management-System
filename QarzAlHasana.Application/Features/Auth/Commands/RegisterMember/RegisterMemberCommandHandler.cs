using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Entities;
using QarzAlHasana.Domain.Exceptions;

namespace QarzAlHasana.Application.Features.Auth.Commands.RegisterMember;

public class RegisterMemberCommandHandler
    : IRequestHandler<RegisterMemberCommand, Guid>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterMemberCommandHandler(
        IMemberRepository memberRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        RegisterMemberCommand request,
        CancellationToken cancellationToken)
    {
        var existingByPhone = await _memberRepository
            .GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);

        if (existingByPhone is not null)
        {
            throw new BusinessRuleException(
                "PHONE_ALREADY_REGISTERED",
                "In shomare telefon ghablan sabt shode ast.");
        }

        var existingByNationalCode = await _memberRepository
            .GetByNationalCodeAsync(request.NationalCode, cancellationToken);

        if (existingByNationalCode is not null)
        {
            throw new BusinessRuleException(
                "NATIONAL_CODE_ALREADY_REGISTERED",
                "In kod-e melli ghablan sabt shode ast.");
        }

        var member = new Member
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            NationalCode = request.NationalCode.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            IsActive = false,
            HasPaidRegistrationFee = false
        };

        await _memberRepository.AddAsync(member, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return member.Id;
    }
}