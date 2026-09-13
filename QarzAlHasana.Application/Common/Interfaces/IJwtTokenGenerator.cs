namespace QarzAlHasana.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string Generate(Guid userId, string role);
}