namespace Application.DTOs.Auth;

public class CurrentUser
{
    public Guid Id { get; set; }
    public string Login { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool IsAuthenticated { get; set; }
}