using MediatR;
using School.Application.Common;
using School.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace School.Application.Commands.DeleteSchool;

public class DeleteSchoolCommandHandler
    : IRequestHandler<DeleteSchoolCommand, Result>
{
    private readonly ISchoolRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IWebHostEnvironment _env;

    public DeleteSchoolCommandHandler(
        ISchoolRepository repository,
        IUnitOfWork uow,
        IWebHostEnvironment env)
    {
        _repository = repository;
        _uow = uow;
        _env = env;
    }

    public async Task<Result> Handle(
        DeleteSchoolCommand request,
        CancellationToken cancellationToken)
    {
        var school = await _repository.GetByIdAsync(request.Id);
        if (school == null)
            return Result.Failure("School not found");

        if (!string.IsNullOrWhiteSpace(school.PhotoUrl))
        {
            var path = Path.Combine(
                _env.WebRootPath,
                school.PhotoUrl.TrimStart('/'));

            if (File.Exists(path))
                File.Delete(path);
        }

        _repository.Remove(school);
        await _uow.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
