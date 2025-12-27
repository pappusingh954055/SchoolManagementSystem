using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);

    // ✅ correct contract
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
}
