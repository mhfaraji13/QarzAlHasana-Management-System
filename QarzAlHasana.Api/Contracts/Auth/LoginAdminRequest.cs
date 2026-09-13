namespace QarzAlHasana.API.Contracts.Auth;

public class LoginAdminRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}