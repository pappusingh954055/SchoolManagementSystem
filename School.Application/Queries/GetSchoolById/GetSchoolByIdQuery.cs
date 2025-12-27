using MediatR;
using School.Application.Common;
using School.Application.DTOs;

namespace School.Application.Queries.GetSchoolById;

public record GetSchoolByIdQuery(Guid Id)
    : IRequest<Result<SchoolResponseDto>>;
