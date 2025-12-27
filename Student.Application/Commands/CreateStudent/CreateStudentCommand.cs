using MediatR;
using Student.Application.Common;
using Student.Application.DTOs;

namespace Student.Application.Commands.CreateStudent;

public record CreateStudentCommand(CreateStudentDto Dto)
    : IRequest<Result<StudentResponseDto>>;
