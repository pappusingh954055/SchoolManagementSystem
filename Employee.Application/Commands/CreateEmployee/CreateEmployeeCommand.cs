using Employee.Application.Common;
using Employee.Application.DTOs;
using MediatR;

namespace Employee.Application.Commands.CreateEmployee;

public record CreateEmployeeCommand(CreateEmployeeRequestDto Dto)
    : IRequest<Result<EmployeeResponseDto>>;
