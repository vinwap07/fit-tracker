namespace Domain.Aggregates;

public class UserAccount: IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Login { get; private set; }
    public string Role { get; private set; }
    
    private string _passwordHash;

    protected UserAccount() { }
    public UserAccount(string email, string login, string passwordHash)
    {
        Id = Guid.NewGuid();
        Email = email;
        Login = login;
        _passwordHash = passwordHash;
    }
    
    public bool VerifyPassword(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.Verify(password, _passwordHash);
    }
}