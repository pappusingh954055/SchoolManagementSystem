using MediatR;
using School.Application.Common;
using School.Application.DTOs;

namespace School.Application.Commands.UpdateSchool;

public record UpdateSchoolCommand(
    Guid Id,
    string Name,
    string Line1,
    string City,
    string State,
    string Country,
    string PostalCode,
    string? PhotoUrl
) : IRequest<Result<SchoolResponseDto>>;
