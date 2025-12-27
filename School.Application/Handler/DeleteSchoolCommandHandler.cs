using MediatR;
using School.Application.Commands.UpdateSchool;
using School.Application.Common;
using School.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Handler
{
    public class DeleteSchoolCommandHandler
     : IRequestHandler<DeleteSchoolCommand, Result<bool>>
    {
        private readonly ISchoolRepository _repository;

        public DeleteSchoolCommandHandler(ISchoolRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(
            DeleteSchoolCommand request,
            CancellationToken cancellationToken)
        {
            var school = await _repository.GetByIdAsync(request.Id);
            if (school == null)
                return Result<bool>.Failure("School not found");

            _repository.Remove(school);
            await _repository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
