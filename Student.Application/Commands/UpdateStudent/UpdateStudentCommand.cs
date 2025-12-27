using MediatR;
using Student.Application.DTOs;
using Student.Application.Common;
namespace Student.Application.Commands.UpdateStudent;

public record UpdateStudentCommand(UpdateStudentDto Dto)
    : IRequest<Result<StudentResponseDto>>;
