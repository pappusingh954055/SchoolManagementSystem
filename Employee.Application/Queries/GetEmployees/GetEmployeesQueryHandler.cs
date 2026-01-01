using Employee.Application.Common;
using Employee.Application.DTOs;
using Employee.Application.Interfaces;
using MediatR;

namespace Employee.Application.Queries.GetEmployees;

public class GetEmployeesQueryHandler
    : IRequestHandler<GetEmployeesQuery, Result<List<EmployeeResponseDto>>>
{
    private readonly IEmployeeRepository _repo;

    public GetEmployeesQueryHandler(IEmployeeRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<List<EmployeeResponseDto>>> Handle(
        GetEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _repo.GetAllAsync();

        var result = list.Select(e => new EmployeeResponseDto
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            Email = e.Email,
            IsActive = e.IsActive
        }).ToList();

        return Result<List<EmployeeResponseDto>>.Success(result);
    }
}
