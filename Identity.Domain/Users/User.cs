using Identity.Domain.Common;
using Identity.Domain.Entities;

namespace Identity.Domain.Users;

public class User : AuditableEntity
{
    private readonly List<UserRole> _userRoles = new();
    private readonly List<RefreshToken> _refreshTokens = new();

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User() { } // EF Core

    public User(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }

    // 🔐 Password
    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    // 🎭 Assign role by RoleId (FK)
    public void AssignRole(int roleId)
    {
        if (_userRoles.Any(r => r.RoleId == roleId))
            return;

        _userRoles.Add(new UserRole(Id, roleId));
    }

    // 🔄 Refresh tokens
    public void AddRefreshToken(RefreshToken token)
    {
        if (_refreshTokens.Any(rt => rt.Token == token.Token))
            return;

        _refreshTokens.Add(token);
    }

    // ✅ Safer revoke (by entity, not string)
    public void RevokeRefreshToken(RefreshToken token)
    {
        if (token == null || !_refreshTokens.Contains(token))
            return;

        token.Revoke();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetModified();
    }
}
