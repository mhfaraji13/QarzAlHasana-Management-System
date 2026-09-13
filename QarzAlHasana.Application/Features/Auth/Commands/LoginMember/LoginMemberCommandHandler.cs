using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Application.Features.Auth.Common;
using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Exceptions;

namespace QarzAlHasana.Application.Features.Auth.Commands.LoginMember;

public class LoginMemberCommandHandler
    : IRequestHandler<LoginMemberCommand, AuthResult>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginMemberCommandHandler(
        IMemberRepository memberRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _memberRepository = memberRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResult> Handle(
        LoginMemberCommand request,
        CancellationToken cancellationToken)
    {
        var member = await _memberRepository
            .GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);

        if (member is null ||
            !_passwordHasher.Verify(request.Password, member.PasswordHash))
        {
            throw new BusinessRuleException(
                "INVALID_CREDENTIALS",
                "Shomare telefon ya ramz-e oboor eshtebah ast.");
        }

        if (!member.IsActive)
        {
            throw new BusinessRuleException(
                "ACCOUNT_NOT_ACTIVE",
                "Hesab-e shoma hanooz tavassot-e modir fa'al nashode ast.");
        }

        var token = _jwtTokenGenerator.Generate(member.Id, Roles.Member);

        return new AuthResult
        {
            UserId = member.Id,
            FullName = member.FirstName + " " + member.LastName,
            Role = Roles.Member,
            Token = token
        };
    }
}