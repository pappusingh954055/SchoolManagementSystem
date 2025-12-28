using Identity.Domain.Common;
using Identity.Domain.Enums;
using Identity.Domain.ValueObjects;

namespace Identity.Domain.Entities;

public class User : AuditableEntity
{
    private readonly List<UserRole> _roles = new();
    private readonly List<RefreshToken> _refreshTokens = new();

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User() { } // EF Core

    public User(string userName, Email email, string passwordHash)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
    }

    // ✅ FIXED: type consistency
    public void AssignRole(UserRoleType role)
    {
        if (_roles.Any(r => r.Name == role.ToString()))
            return;

        _roles.Add(new UserRole(role.ToString()));
    }

    public void AddRefreshToken(RefreshToken token)
    {
        _refreshTokens.Add(token);
    }

    public void RemoveRefreshToken(string token)
    {
        var refreshToken = _refreshTokens.Single(rt => rt.Token == token);
        refreshToken.Revoke();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetModified();
    }
}
