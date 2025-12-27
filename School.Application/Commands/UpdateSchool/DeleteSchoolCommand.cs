using MediatR;
using School.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Commands.UpdateSchool
{
    public record DeleteSchoolCommand(Guid Id)
    : IRequest<Result<bool>>;
}
