using Identity.Application.Common;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Domain.ValueObjects;
using MediatR;

namespace Identity.Application.Commands.RegisterUser;

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            return Result<AuthResponseDto>.Failure("User already exists");

        var email = Email.Create(dto.Email);
        var passwordHash = _passwordHasher.Hash(dto.Password);

        var user = new Domain.Entities.User(dto.UserName, email, passwordHash);
        user.AssignRole(dto.Role);

        await _userRepository.AddAsync(user);

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        refreshToken.AssignUser(user.Id);

        await _userRepository.AddRefreshTokenAsync(refreshToken);

        return Result<AuthResponseDto>.Success(
            new AuthResponseDto(
                user.Id,
                user.UserName,
                user.Email.Value,
                user.Roles.Select(r => r.Name),
                accessToken,
                refreshToken.Token
            ));
    }
}
