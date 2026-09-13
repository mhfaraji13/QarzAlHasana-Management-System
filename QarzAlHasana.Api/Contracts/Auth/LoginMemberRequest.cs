namespace QarzAlHasana.API.Contracts.Auth;

public class LoginMemberRequest
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}