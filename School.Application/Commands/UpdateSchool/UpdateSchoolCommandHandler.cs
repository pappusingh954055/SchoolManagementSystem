using MediatR;
using School.Application.Common;
using School.Application.DTOs;
using School.Application.Interfaces;
using School.Domain.ValueObjects;

namespace School.Application.Commands.UpdateSchool;

public class UpdateSchoolCommandHandler
    : IRequestHandler<UpdateSchoolCommand, Result<SchoolResponseDto>>
{
    private readonly ISchoolRepository _repository;

    public UpdateSchoolCommandHandler(ISchoolRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SchoolResponseDto>> Handle(
        UpdateSchoolCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var school = await _repository.GetByIdAsync(dto.Id);
        if (school == null)
            return Result<SchoolResponseDto>.Failure("School not found");

        // ✅ VALUE OBJECT (Address)
        var address = Address.Create(
            dto.Line1,
            dto.City,
            dto.State,
            dto.Country,
            dto.PostalCode
        );

        // ✅ DOMAIN STATE UPDATE
        school.UpdateName(dto.Name);
        school.UpdateAddress(address);

        // ⚠ PhotoUrl is updated in API layer (after upload)
        // school.UpdatePhoto(dto.PhotoUrl);  <-- only if you pass it via command

        await _repository.SaveChangesAsync();

        // ✅ RESPONSE DTO (READ FROM AGGREGATE)
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
