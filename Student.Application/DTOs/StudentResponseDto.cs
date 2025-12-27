namespace Student.Application.DTOs;

public record StudentResponseDto(
    Guid Id,
    string StudentCode,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Gender,
    Guid SchoolId,
    string Line1,
    string City,
    string State,
    string Country,
    string PostalCode,
    string? PhotoUrl
);
