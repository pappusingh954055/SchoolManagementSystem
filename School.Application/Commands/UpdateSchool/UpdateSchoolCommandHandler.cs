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
    private readonly IUnitOfWork _uow;

    public UpdateSchoolCommandHandler(
        ISchoolRepository repository,
        IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<Result<SchoolResponseDto>> Handle(
     UpdateSchoolCommand request,
     CancellationToken cancellationToken)
    {
        var school = await _repository.GetByIdAsync(request.Id);
        if (school == null)
            return Result<SchoolResponseDto>.Failure("School not found");

        var address = Address.Create(
            request.Line1,
            request.City,
            request.State,
            request.Country,
            request.PostalCode
        );

        school.UpdateName(request.Name);
        school.UpdateAddress(address);

        if (!string.IsNullOrWhiteSpace(request.PhotoUrl))
            school.UpdatePhoto(request.PhotoUrl);

        await _uow.CommitAsync(cancellationToken);

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
