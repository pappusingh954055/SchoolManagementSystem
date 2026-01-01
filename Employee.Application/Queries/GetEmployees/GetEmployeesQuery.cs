using Employee.Application.Common;
using Employee.Application.DTOs;
using MediatR;

namespace Employee.Application.Queries.GetEmployees;

public record GetEmployeesQuery
    : IRequest<Result<List<EmployeeResponseDto>>>;
