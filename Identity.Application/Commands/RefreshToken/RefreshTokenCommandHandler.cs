using Identity.Application.Common;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Commands.RefreshToken;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _uow;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IJwtService jwtService,
        IUnitOfWork uow)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _uow = uow;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByRefreshTokenAsync(request.RefreshToken);

        if (user == null)
            return Result<AuthResponseDto>.Failure("Invalid refresh token");

        var oldToken = user.RefreshTokens
            .Single(rt => rt.Token == request.RefreshToken);

        if (!oldToken.IsActive)
            return Result<AuthResponseDto>.Failure("Refresh token expired");

        // 🔄 Revoke old token
        oldToken.Revoke();

        // 🔐 Generate new tokens
        var roles = user.UserRoles
            .Select(ur => ur.Role.RoleName)
            .ToList();

        var auth = _jwtService.Generate(user, roles);

        // ✅ Persist new refresh token
        user.AddRefreshToken(new Domain.Entities.RefreshToken(
            auth.RefreshToken,
            auth.ExpiresAt,
            user.Id
        ));

        await _uow.SaveChangesAsync(cancellationToken);

        // ✅ MAP → API RESPONSE DTO
        return Result<AuthResponseDto>.Success(
            new AuthResponseDto
            {
                UserId = user.Id,
                AccessToken = auth.AccessToken,
                RefreshToken = auth.RefreshToken,
                Roles = roles
            });
    }
}
