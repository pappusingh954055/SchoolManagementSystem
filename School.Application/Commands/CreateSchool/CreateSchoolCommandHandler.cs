using MediatR;
using School.Application.Common;
using School.Application.Interfaces;
using School.Domain.ValueObjects;

namespace School.Application.Commands.CreateSchool;

public class CreateSchoolCommandHandler
    : IRequestHandler<CreateSchoolCommand, Result<SchoolResponseDto>>
{
    private readonly ISchoolRepository _repository;
    private readonly IUnitOfWork _uow;

    public CreateSchoolCommandHandler(
        ISchoolRepository repository,
        IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<Result<SchoolResponseDto>> Handle(
        CreateSchoolCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // ✅ Uniqueness check
        if (await _repository.ExistsByCodeAsync(dto.Code))
            return Result<SchoolResponseDto>.Failure("School code already exists");

        // ✅ VALUE OBJECTS
        var schoolCode = new SchoolCode(dto.Code);

        var address = Address.Create(
            dto.Line1,
            dto.City,
            dto.State,
            dto.Country,
            dto.PostalCode
        );

        // ✅ ENTITY (KEEPING YOUR CONSTRUCTOR)
        var school = new School.Domain.Entities.School(
            schoolCode.Value,
            dto.Name,
            address,
            dto.PhotoUrl
        );

        await _repository.AddAsync(school);

        // ✅ SINGLE COMMIT (FIX)
        await _uow.CommitAsync(cancellationToken);

        // ✅ RESPONSE DTO
        return Result<SchoolResponseDto>.Success(
            new SchoolResponseDto(
                school.Id,
                school.Code,
                school.Name,
                school.Address.Line1,
                school.Address.City,
                school.Address.State,
                school.Address.Country,
                school.Address.PostalCode,
                school.PhotoUrl
            )
        );
    }
}
