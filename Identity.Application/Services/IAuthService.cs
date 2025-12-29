using Identity.Application.DTOs;

namespace Identity.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginDto dto);
    }
}
