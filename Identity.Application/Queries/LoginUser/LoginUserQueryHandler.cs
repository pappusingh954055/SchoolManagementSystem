using Identity.Application.Common;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Application.Queries.LoginUser;
using Identity.Domain.Entities;
using Identity.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;

public class LoginUserQueryHandler
    : IRequestHandler<LoginUserQuery, Result<AuthResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserQueryHandler(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        IJwtService jwtService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResponse>> Handle(
        LoginUserQuery request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var user = await _userRepository
            .GetWithRolesByEmailAsync(dto.Email);

        if (user == null)
            return Result<AuthResponse>.Failure("Invalid credentials");

        var verifyResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            dto.Password);

        if (verifyResult == PasswordVerificationResult.Failed)
            return Result<AuthResponse>.Failure("Invalid credentials");

        // 🔄 Revoke existing active refresh tokens
        foreach (var token in user.RefreshTokens.Where(t => t.IsActive))
            token.Revoke();

        var roles = user.UserRoles
            .Select(ur => ur.Role.RoleName)
            .ToList();

        // 🔐 Generate tokens
        var auth = _jwtService.Generate(user, roles);

        // ➕ Add new refresh token
        user.AddRefreshToken(
            new RefreshToken(
                auth.RefreshToken,
                auth.ExpiresAt.AddDays(7),
                auth.UserId
            )
        );

        // ✅ SINGLE SAVE (UnitOfWork only)
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(auth);
    }
}
