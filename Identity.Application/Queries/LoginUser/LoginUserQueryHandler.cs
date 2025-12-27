using Identity.Application.Common;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Queries.LoginUser;

public class LoginUserQueryHandler
    : IRequestHandler<LoginUserQuery, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUserQueryHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        LoginUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null || !user.IsActive)
            return Result<AuthResponseDto>.Failure("Invalid credentials");

        if (!_passwordHasher.Verify(user.PasswordHash, request.Password))
            return Result<AuthResponseDto>.Failure("Invalid credentials");

        // Generate tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Attach FK correctly
        refreshToken.AssignUser(user.Id);
        await _userRepository.AddRefreshTokenAsync(refreshToken);

        // ✅ KEY FIX: project role names safely
        var roleNames = user.Roles
            .Select(r => r.Name)
            .ToList();

        return Result<AuthResponseDto>.Success(
            new AuthResponseDto(
                user.Id,
                user.UserName,
                user.Email.Value,
                roleNames,
                accessToken,
                refreshToken.Token
            ));
    }
}
