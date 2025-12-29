using Identity.Application.Common;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using MediatR;

namespace Identity.Application.Commands.RefreshToken;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
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

    public async Task<Result<AuthResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByRefreshTokenAsync(request.RefreshToken);

        if (user == null)
            return Result<AuthResponse>.Failure("Invalid refresh token");

        var refreshToken = user.RefreshTokens
            .Single(rt => rt.Token == request.RefreshToken);

        if (!refreshToken.IsActive)
            return Result<AuthResponse>.Failure("Refresh token expired");

        // 🔄 Revoke old token
        refreshToken.Revoke();

        var roles = user.UserRoles
            .Select(ur => ur.Role.RoleName)
            .ToList();

        var auth = _jwtService.Generate(user, roles);

        // 🔁 Rotate refresh token
        user.AddRefreshToken(
            auth.RefreshToken,
            auth.ExpiresAt.AddDays(7)
        );

        await _uow.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(auth);
    }
}
