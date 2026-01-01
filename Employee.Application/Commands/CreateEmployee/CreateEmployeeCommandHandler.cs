using Employee.Application.Common;
using Employee.Application.DTOs;
using Employee.Application.Interfaces;
using Employee.Domain.Entities;
using MediatR;

namespace Employee.Application.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, Result<EmployeeResponseDto>>
{
    private readonly IEmployeeRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository repo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Result<EmployeeResponseDto>> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var e = new Domain.Entities.Employee(
            request.Dto.Code,
            request.Dto.Name,
            request.Dto.Email);

        await _repo.AddAsync(e);
        await _uow.CommitAsync(cancellationToken);

        return Result<EmployeeResponseDto>.Success(new EmployeeResponseDto
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            Email = e.Email,
            IsActive = e.IsActive
        });
    }
}
