using MediatR;
using School.Application.Common;
using School.Application.DTOs;
using School.Application.Interfaces;

namespace School.Application.Queries.GetSchoolById;

public class GetSchoolByIdQueryHandler
    : IRequestHandler<GetSchoolByIdQuery, Result<SchoolResponseDto>>
{
    private readonly ISchoolRepository _repository;

    public GetSchoolByIdQueryHandler(ISchoolRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SchoolResponseDto>> Handle(
        GetSchoolByIdQuery request,
        CancellationToken cancellationToken)
    {
        var school = await _repository.GetByIdAsync(request.Id);
        if (school == null)
            return Result<SchoolResponseDto>.Failure("School not found");

        return Result<SchoolResponseDto>.Success(
            new SchoolResponseDto(
                school.Id,
                school.Name,
                school.Code,
                school.Address.Line1,
                school.Address.City,
                school.Address.State,
                school.Address.Country,
                school.Address.PostalCode,
                school.PhotoUrl
            ));
    }
}
