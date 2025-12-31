using MediatR;
using School.Application.Common;

namespace School.Application.Commands.DeleteSchool;

public record DeleteSchoolCommand(Guid Id)
    : IRequest<Result>;
