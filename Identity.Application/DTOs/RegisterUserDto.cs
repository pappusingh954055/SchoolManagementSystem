using Identity.Domain.Enums;

namespace Identity.Application.DTOs;

public record RegisterUserDto(
    string UserName,
    string Email,
    string Password,
    UserRoleType Role
);
