namespace Identity.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }

    // FK
    public Guid UserId { get; private set; }

    // Navigation
    public User? User { get; private set; }

    private RefreshToken() { }

    public RefreshToken(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsExpired && !IsRevoked;

    public void AssignUser(Guid userId)
    {
        UserId = userId;
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}
