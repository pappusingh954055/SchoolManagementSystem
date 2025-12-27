using Identity.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Security;

public class IdentityPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool Verify(string hash, string password)
    {
        return _hasher.VerifyHashedPassword(null!, hash, password)
            == PasswordVerificationResult.Success;
    }
}
