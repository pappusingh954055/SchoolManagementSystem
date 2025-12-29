using Identity.Application.DTOs;
using Identity.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Interfaces
{
    public interface IJwtService
    {
        AuthResponse Generate(User user, List<string> roles);
    }
}
