using MediatR;
using Student.Application.Common;
using Student.Application.Interfaces;

namespace Student.Application.Commands.DeleteStudent;

public class DeleteStudentCommandHandler
    : IRequestHandler<DeleteStudentCommand, Result>
{
    private readonly IStudentRepository _repository;

    public DeleteStudentCommandHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        DeleteStudentCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _repository.GetByIdAsync(request.Id);
        if (student == null)
            return Result.Failure("Student not found");

        _repository.Remove(student);
        await _repository.SaveChangesAsync();

        return Result.Success();
    }
}
