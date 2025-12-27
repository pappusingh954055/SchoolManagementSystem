using MediatR;
using Student.Application.DTOs;
using Student.Application.Interfaces;
using Student.Domain.ValueObjects;
using Student.Application.Common;
namespace Student.Application.Commands.UpdateStudent;

public class UpdateStudentCommandHandler
    : IRequestHandler<UpdateStudentCommand, Result<StudentResponseDto>>
{
    private readonly IStudentRepository _repository;

    public UpdateStudentCommandHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<StudentResponseDto>> Handle(
        UpdateStudentCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var student = await _repository.GetByIdAsync(dto.Id);
        if (student == null)
            return Result<StudentResponseDto>.Failure("Student not found");

        // ✅ Update domain state
        student.UpdateName(dto.FirstName, dto.LastName);

        var address = Address.Create(
            dto.Line1,
            dto.City,
            dto.State,
            dto.Country,
            dto.PostalCode
        );

        student.UpdateAddress(address);

        if (!string.IsNullOrWhiteSpace(dto.PhotoUrl))
            student.UpdatePhoto(dto.PhotoUrl);

        await _repository.SaveChangesAsync();

        return Result<StudentResponseDto>.Success(
            new StudentResponseDto(
                student.Id,
                student.StudentCode,
                student.FirstName,
                student.LastName,
                student.DateOfBirth,
                student.Gender,
                student.SchoolId,
                student.Address.Line1,
                student.Address.City,
                student.Address.State,
                student.Address.Country,
                student.Address.PostalCode,
                student.PhotoUrl
            )
        );
    }
}
