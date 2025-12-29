namespace Identity.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }

    // FK
    public Guid UserId { get; private set; }
    public User? User { get; private set; }

    private RefreshToken() { } // EF Core

    public RefreshToken(string token, DateTime expiresAt, Guid userId)
    {
        Token = token;
        ExpiresAt = expiresAt;
        UserId = userId;
        IsRevoked = false;
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsExpired && !IsRevoked;

    public void Revoke()
    {
        IsRevoked = true;
    }
}
