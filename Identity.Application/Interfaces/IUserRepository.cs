using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface IUserRepository
{

    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
   
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
    // ✅ correct contract
    Task AddRefreshTokenAsync(RefreshToken refreshToken);

    Task AddAsync(User user);

    Task SaveChangesAsync();
}
