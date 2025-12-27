using MediatR;
using Student.Application.Common;
using Student.Application.DTOs;
using Student.Application.Interfaces;
using Student.Domain.ValueObjects;

namespace Student.Application.Commands.CreateStudent;

public class CreateStudentCommandHandler
    : IRequestHandler<CreateStudentCommand, Result<StudentResponseDto>>
{
    private readonly IStudentRepository _repository;

    public CreateStudentCommandHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<StudentResponseDto>> Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // ✅ Uniqueness check
        if (await _repository.ExistsByStudentCodeAsync(dto.StudentCode))
            return Result<StudentResponseDto>.Failure("Student code already exists");

        // ✅ Value Object
        var address = Address.Create(
            dto.Line1,
            dto.City,
            dto.State,
            dto.Country,
            dto.PostalCode
        );

        // ✅ Entity
        var student = new Student.Domain.Entities.Student(
            dto.StudentCode,
            dto.FirstName,
            dto.LastName,
            dto.DateOfBirth,
            dto.Gender,
            dto.SchoolId,
            address,
            dto.PhotoUrl
        );

        await _repository.AddAsync(student);
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
