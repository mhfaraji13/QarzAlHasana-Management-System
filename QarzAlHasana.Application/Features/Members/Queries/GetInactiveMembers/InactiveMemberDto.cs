namespace QarzAlHasana.Application.Features.Members.Queries.GetInactiveMembers;

public class InactiveMemberDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}