using Identity.Application.Common;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Commands.RefreshToken;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByRefreshTokenAsync(request.RefreshToken);

        if (user == null)
            return Result<AuthResponseDto>.Failure("Invalid refresh token");

        var refreshToken = user.RefreshTokens
            .Single(rt => rt.Token == request.RefreshToken);

        if (!refreshToken.IsActive)
            return Result<AuthResponseDto>.Failure("Refresh token expired");

        // 🔐 Rotate tokens
        user.RemoveRefreshToken(refreshToken.Token);

        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.AddRefreshToken(newRefreshToken);

        var accessToken = _tokenService.GenerateAccessToken(user);

        await _userRepository.SaveChangesAsync();

        return Result<AuthResponseDto>.Success(
            new AuthResponseDto(
                user.Id,
                user.UserName,
                user.Email.Value,
                user.Roles.Select(r => r.Name),
                accessToken,
                newRefreshToken.Token
            ));
    }
}
