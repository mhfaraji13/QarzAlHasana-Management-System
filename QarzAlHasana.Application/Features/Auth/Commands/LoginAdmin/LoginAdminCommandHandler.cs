using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Application.Features.Auth.Common;
using QarzAlHasana.Domain.Common;
using QarzAlHasana.Domain.Exceptions;

namespace QarzAlHasana.Application.Features.Auth.Commands.LoginAdmin;

public class LoginAdminCommandHandler
    : IRequestHandler<LoginAdminCommand, AuthResult>
{
    private readonly IAdminRepository _adminRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginAdminCommandHandler(
        IAdminRepository adminRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _adminRepository = adminRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResult> Handle(
        LoginAdminCommand request,
        CancellationToken cancellationToken)
    {
        var admin = await _adminRepository
            .GetByEmailAsync(request.Email, cancellationToken);

        if (admin is null ||
            !_passwordHasher.Verify(request.Password, admin.PasswordHash))
        {
            throw new BusinessRuleException(
                "INVALID_CREDENTIALS",
                "Email ya ramz-e oboor eshtebah ast.");
        }

        var token = _jwtTokenGenerator.Generate(admin.Id, Roles.Admin);

        return new AuthResult
        {
            UserId = admin.Id,
            FullName = admin.FullName,
            Role = Roles.Admin,
            Token = token
        };
    }
}