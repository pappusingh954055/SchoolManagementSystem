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
        var user = await _userRepository
            .GetWithRolesByEmailAsync(request.Dto.Email);

        if (user == null)
            return Result<AuthResponse>.Failure("Invalid credentials");

        var verify = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Dto.Password);

        if (verify == PasswordVerificationResult.Failed)
            return Result<AuthResponse>.Failure("Invalid credentials");

        var roles = user.UserRoles
            .Select(r => r.Role.RoleName)
            .ToList();

        var auth = _jwtService.Generate(user, roles);

        // ✅ FIXED
        user.AddRefreshToken(
            auth.RefreshToken,
            auth.ExpiresAt.AddDays(7)
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AuthResponse>.Success(auth);
    }
}
