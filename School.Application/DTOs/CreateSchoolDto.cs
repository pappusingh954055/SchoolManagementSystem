namespace School.Application.DTOs;

public record CreateSchoolDto(
    string Code,
    string Name,
    string Line1,
    string City,
    string State,
    string Country,
    string PostalCode,
    string? PhotoUrl
);

