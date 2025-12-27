using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs
{  
    public record UpdateSchoolDto(
    Guid Id,
    string Name,
    string Line1,
    string City,
    string State,
    string Country,
    string PostalCode,
    string? PhotoUrl
);
}
