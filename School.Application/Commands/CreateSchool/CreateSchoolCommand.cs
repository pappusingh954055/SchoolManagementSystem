using MediatR;
using School.Application.Common;
using School.Application.DTOs;

namespace School.Application.Commands.CreateSchool;

public record CreateSchoolCommand(
    CreateSchoolDto Dto
) : IRequest<Result<SchoolResponseDto>>;
