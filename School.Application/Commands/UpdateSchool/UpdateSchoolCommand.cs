using MediatR;
using School.Application.Common;
using School.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Commands.UpdateSchool
{
    public record UpdateSchoolCommand(UpdateSchoolDto Dto)
    : IRequest<Result<SchoolResponseDto>>;
}
