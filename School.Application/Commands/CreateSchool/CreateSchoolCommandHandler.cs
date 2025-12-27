using MediatR;
using School.Application.Common;
using School.Application.DTOs;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.ValueObjects;

namespace School.Application.Commands.CreateSchool;

public class CreateSchoolCommandHandler
    : IRequestHandler<CreateSchoolCommand, Result<SchoolResponseDto>>
{
    private readonly ISchoolRepository _repository;

    public CreateSchoolCommandHandler(ISchoolRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SchoolResponseDto>> Handle(
        CreateSchoolCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // ✅ uniqueness check
        if (await _repository.ExistsByCodeAsync(dto.Code))
            return Result<SchoolResponseDto>.Failure("School code already exists");

        // ✅ VALUE OBJECT
        var code = new SchoolCode(dto.Code);

        // ✅ ENTITY (correct constructor)
        var school = new Domain.Entities.School(
            code,
            dto.Name,dto.Line1,dto.City,dto.State,dto.Country,dto.PostalCode
        );

        await _repository.AddAsync(school);
        await _repository.SaveChangesAsync();

        // ✅ RESPONSE
        return Result<SchoolResponseDto>.Success(
            new SchoolResponseDto(
                school.Id,
                school.Name,
                school.Code.Value,school.Line1,school.City,school.State,school.Country,school.PostalCode
            )
        );
    }
}
