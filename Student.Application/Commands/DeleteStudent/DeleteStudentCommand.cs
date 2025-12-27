using MediatR;
using Student.Application.Common;
namespace Student.Application.Commands.DeleteStudent;

public record DeleteStudentCommand(Guid Id)
    : IRequest<Result>;
