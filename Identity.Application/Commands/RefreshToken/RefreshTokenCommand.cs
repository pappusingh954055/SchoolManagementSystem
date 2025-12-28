using Identity.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken)
     : IRequest<Result<AuthResponseDto>>;
}
